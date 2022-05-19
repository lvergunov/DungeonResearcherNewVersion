using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using System.Drawing;
using GameClasses.GameMap;
using GameClasses.ProcedureGeneration;
using GameClasses.Beings;
using GameClasses.Beings.Decorators;
using GameClasses.Enums;
using GameClasses.Weapons;
using GameClasses.Weapons.ColdWeapons;
using GameClasses.Weapons.ShootingWeapons;
using GameClasses.Factory.BeingsFactory;
using GameClasses.Factory.DecoratorFactory;
using GameClasses.Factory.StuffFactory;
using GameClasses.Weapons.ShootingWeapons.Bullets;
using GameClasses.Game;
using GameClasses.GameObjects;
using GameClasses.GameObjects.Chests;
using GameClasses.GameObjects.LetsAndTraps;

namespace DungeonResearcher.RenderingProcess
{
    public class GameCycle
    {
        public delegate void RewardEventHandler(string paramName);
        public event RewardEventHandler onReward;

        public delegate void SignalizeDelegate();
        public event SignalizeDelegate onEndGame;

        public RectangularGameMap GameMap { get; protected set; }
        public Hero Player { get; protected set; }
        public bool IsPlaying { get; private set; } = false;
        public int Level { get; protected set; }
        public bool IsLevelLoaded { get; protected set; } = false;
        public int DecorationValue { get; protected set; } = 10;
        public GameCycle()
        {
        }

        public void LoadGame()
        {
            Level = 1;
            Room heroSpawnRoom = CreateLevel();
            Random randomiser = new Random();
            int heroColiderWidth = 10;
            int heroColiderHeight = 20;
            Point spawnPoint = new Point(randomiser.Next(heroSpawnRoom.Colider.Left + heroColiderWidth,
            heroSpawnRoom.Colider.Right - heroColiderWidth), randomiser.Next(heroSpawnRoom.Colider.Top + heroColiderHeight,
            heroSpawnRoom.Colider.Bottom - heroColiderHeight));
            Player = new Hero(heroSpawnRoom, 10, 20,
                spawnPoint, heroColiderWidth, heroColiderHeight, "Hero",
                new WeaponFactory().GetOldSword(Point.Empty), 5, 100);
            Player.onRelocation += AttackHero;
            Player.onRelocation += StartTrapRedrawing;
            Player.beingIsDead += StopGame;
            Player.onWeaponTook += TakeNewWeapon;
            foreach (Room room in GameMap.Insertions)
            {
                if (room is DangerousRoom dangerousRoom)
                {
                    foreach (Enemy enemy in dangerousRoom.Enemies)
                    {
                        enemy.beingIsDead += RewardHero;
                        enemy.SetTarget(Player);
                        if (enemy.WeaponInHands is ColdWeapon coldWeapon)
                            coldWeapon.onColdWeaponPunch += RegistrateEnemyColdPunch;
                        else if (enemy.WeaponInHands is ShootingWeapon shootingWeapon)
                            shootingWeapon.onShootingPunch += RegistrateEnemyShooting;
                    }
                }
            }
            foreach (IWeapon weapon in Player.Arsenal)
            {
                if (weapon is ColdWeapon herosCold)
                    herosCold.onColdWeaponPunch += RegistrateHeroColdPunch;
                else if (weapon is ShootingWeapon shooting)
                    shooting.onShootingPunch += RegistrateHeroShooting;
            }
            var exits = from exit in GameMap.Coridors
                        where exit.InnerRoom == Player.NativeRoom
                        select exit;
            while (Player.NativeRoom.Stuffs.Any(st => st.Colider.IntersectsWith(Player.Colider))
                && exits.Any(ex => ex.Colider.IntersectsWith(Player.Colider)))
            {
                Thread.Sleep(10);
                int randomX = randomiser.Next(Player.NativeRoom.Colider.Left, Player.NativeRoom.Colider.Right
                    - Player.Colider.Width);
                Thread.Sleep(10);
                int randomY = randomiser.Next(Player.NativeRoom.Colider.Top, Player.NativeRoom.Colider.Bottom -
                    Player.Colider.Height);
                StopTrapRedrawing();
                Player.ChangePosition(new Point(randomX, randomY));
                StartTrapRedrawing();
            }
            IsLevelLoaded = true;
        }

        public Room CreateLevel()
        {
            Random random = new Random();
            int depthOfGenerationTree = random.Next(4, 6);
            Thread.Sleep(10);
            GameMap = new RectangularGameMap(Level, depthOfGenerationTree, 3000, 2000, Orientation.Vertical);
            ProcedureGenerator generator = new ProcedureGenerator(GameMap, 300, 200, 500, 400);
            Room heroSpawnRoom;
            generator.CreateLevel(out heroSpawnRoom);
            return heroSpawnRoom;
        }
        public void StartGame()
        {
            IsPlaying = true;
            StartTrapRedrawing();
            try
            {
                if (Player.NativeRoom.Stuffs.Any(st => st is ITrap tr && st.Colider.IntersectsWith(Player.Colider)
                && !tr.IsReloading))
                {
                    var colidedTraps = from trap in Player.NativeRoom.Stuffs
                                       where trap.Colider.IntersectsWith(Player.Colider) && trap is ITrap tr
                                       && !tr.IsReloading
                                       select trap;
                    (colidedTraps.First() as ITrap).PunchHero(Player);
                    Player.GetPunched((colidedTraps.First() as ITrap).Damage);
                }
            }
            catch { }
            var exits = from ex in GameMap.Coridors
                        where ex.InnerRoom == Player.NativeRoom
                        select ex;
            Player.NativeRoom.Stuffs.RemoveAll(s => s is IDestroyable ds && !ds.IsAlive);
            if (IsHeroRelocated(out Room roomToReloc,
                out RoomExit newExit))
            {
                StopTrapRedrawing();
                Player.Relocate(roomToReloc, newExit);
                StartTrapRedrawing();
            }
            if (Player.Colider.IntersectsWith(GameMap.Exit.Colider))
            {
                Level++;
                IsLevelLoaded = false;
                Task<Room> newLevel = new Task<Room>(() => CreateLevel());
                newLevel.Start();
                Player.ChangeRoom(newLevel.Result);
            }
            if (Player.NativeRoom is DangerousRoom dangerous &&
            dangerous.Enemies.Count != 0 && dangerous.Enemies.Any(en => en.IsAlive))
                exits.ToList().ForEach(ex => ex.CloseExit());
        }
        public void StopGame()
        {
            IsPlaying = false;
            onEndGame();
        }


        public void HerosInteractiveAction()
        {
            var nearestInteractiveObjects = from stuff in Player.NativeRoom.Stuffs
                                            where stuff is IInteractiveObject interactive &&
                                            interactive.ColiderOfInteraction.IntersectsWith(Player.Colider)
                                            select stuff;
            if (nearestInteractiveObjects.Count() != 0)
            {
                Thread thread = new Thread(() =>
                (nearestInteractiveObjects.First() as IInteractiveObject).Use(Player));
                thread.Start();
            }
        }

        protected void TakeNewWeapon(IWeapon weapon)
        {
            if (weapon is ColdWeapon coldWeapon)
                coldWeapon.onColdWeaponPunch += RegistrateHeroColdPunch;
            else if (weapon is ShootingWeapon shootingWeapon)
                shootingWeapon.onShootingPunch += RegistrateHeroShooting;

        }

        protected void StartTrapRedrawing()
        {
            try
            {
                foreach (IGenerable obj in Player.NativeRoom.Stuffs)
                {
                    if (obj is ITrap trap)
                    {
                        Thread thread = new Thread(() => trap.StartCycle());
                        thread.Start();
                    }
                }
            }
            catch { StartTrapRedrawing(); }
        }

        protected void StopTrapRedrawing()
        {
            foreach (IGenerable obj in Player.NativeRoom.Stuffs)
            {
                if (obj is ITrap trap)
                {
                    Thread thread = new Thread(() => trap.StopCycle());
                    thread.Start();
                }
            }
        }
        protected void RegistrateHeroColdPunch(Being hero)
        {
            if (hero.TryGetNearestObject(hero.WeaponInHands.MaxDamageRadius, out IObjectOnGameMap mapObject)
                && mapObject is IDestroyable destroyableObj)
                destroyableObj.GetPunched(hero.WeaponInHands.CountDamage(hero), hero);
        }
        protected void RegistrateHeroShooting(Being hero)
        {
            BulletInFlight herosBullet = new BulletInFlight(hero, hero.Direction, (hero.WeaponInHands as ShootingWeapon).ReadyBullet);
            hero.NativeRoom.Add(herosBullet);
            Thread thread = new Thread(() => herosBullet.Loose(hero.NativeRoom));
            thread.Start();
        }
        protected void RegistrateEnemyColdPunch(Being enemy)
        {
            if (enemy.TryGetNearestObject(enemy.WeaponInHands.MaxDamageRadius, out IObjectOnGameMap gameObject)
                && gameObject == Player)
                Player.GetPunched(enemy.WeaponInHands.CountDamage(enemy), enemy);
        }
        protected void RegistrateEnemyShooting(Being enemy)
        {
            BulletInFlight enemyBullet = new BulletInFlight(enemy, enemy.Direction, (enemy.WeaponInHands as ShootingWeapon).ReadyBullet);
            Player.NativeRoom.Add(enemyBullet);
            Thread thread = new Thread(() => enemyBullet.Loose(Player.NativeRoom));
            thread.Start();
        }


        protected void AttackHero()
        {
            foreach (Enemy en in (Player.NativeRoom as DangerousRoom).Enemies)
            {
                Thread thread = new Thread(() => en.SearchForHero(Player));
                thread.Start();
            }
        }
        protected bool IsHeroRelocated(out Room newRoom, out RoomExit newExit)
        {
            var exitsLinq = from exit in GameMap.Coridors
                            where exit.InnerRoom == Player.NativeRoom &&
                            exit.Colider.IntersectsWith(Player.Colider)
                            && exit.IsOpened
                            select exit;
            if (exitsLinq.Count() != 0)
            {
                newRoom = exitsLinq.First().OuterRoom;
                var newExitLinq = from exit in GameMap.Coridors
                                  where exit.InnerRoom == exitsLinq.First().OuterRoom
                                  && exit.OuterRoom == Player.NativeRoom
                                  select exit;
                newExit = newExitLinq.First();
                return true;
            }
            else
            {
                newRoom = null;
                newExit = null;
                return false;
            }
        }


        protected void RewardHero()
        {
            if (Player.NativeRoom is DangerousRoom dangerousRoom &&
                dangerousRoom.Enemies.All(en => !en.IsAlive))
            {
                DecoratorsFactory decoratorsFactory = new DecoratorsFactory();
                Player.onRelocation -= AttackHero;
                Player.onRelocation -= StartTrapRedrawing;
                Player.onWeaponTook -= TakeNewWeapon;
                Player.beingIsDead -= StopGame;
                Player = decoratorsFactory.CreateDecorator(Player, DecorationValue);
                var exits = from exit in GameMap.Coridors
                            where exit.InnerRoom == Player.NativeRoom
                            select exit;
                exits.ToList().ForEach(exit => exit.OpenExit());
                onReward((Player as HeroDecorator).ParamName);
                foreach (Room room in GameMap.Insertions)
                {
                    if (room is DangerousRoom dr)
                    {
                        foreach (Enemy enemy in dr.Enemies)
                        {
                            enemy.SetTarget(Player);
                        }
                    }
                }
                Player.onRelocation += AttackHero;
                Player.onRelocation += StartTrapRedrawing;
                Player.onWeaponTook += TakeNewWeapon;
                Player.beingIsDead += StopGame;
            }
        }
    }
}

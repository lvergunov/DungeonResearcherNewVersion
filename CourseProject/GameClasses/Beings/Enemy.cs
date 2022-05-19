using GameClasses.GameMap;
using GameClasses.Weapons;
using GameClasses.GameObjects;
using GameClasses.Enums;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameClasses.Game;
using System;
using System.Collections.Generic;

namespace GameClasses.Beings
{
    public class Enemy : Being
    {
        public Rectangle DamageZone { get; protected set; }
        public Enemy(EnemyType enemyType,Room spawnRoom, IWeapon weapon, float minDamage,
            float maxDamage, Point position, int coliderWidth, int coliderHeight,
            string name, float speed, RarityOfStuff rarity, float health)
            : base(spawnRoom, weapon, minDamage, maxDamage, position, coliderWidth,
                  coliderHeight, name, speed, health)
        {
            Rarity = rarity;
            TypeOfEnemy = enemyType;
        }

        public Being Target { get; protected set; }

        EnemyType TypeOfEnemy { get; }

        public virtual void SearchForHero(Hero hero)
        {
            Random random = new Random();
            SinchronizeWeapon();
            while (IsAlive && hero.IsAlive)
            {
                DamageZone = GetPathColider(WeaponInHands.MaxDamageRadius);
                if (hero.Colider.IntersectsWith(DamageZone))
                    Punch();
                else
                {
                    if (AreOnOneLine(out Orientation orientation))
                    {
                        if (orientation == Orientation.Vertical)
                            Move(GetRelationsWithHeroOnVertical(), (int)SpeedPerMS);
                        else if (orientation == Orientation.Horizontal)
                            Move(GetRelationsWithHeroOnHorizontal(), (int)SpeedPerMS);
                    }
                    else {
                        Directions[] directions = new Directions[2];
                        directions[0] = GetRelationsWithHeroOnHorizontal();
                        directions[1] = GetRelationsWithHeroOnVertical();
                        Thread.Sleep(10);
                        int numberOfSelected = random.Next(0,directions.Length);
                        Move(directions[numberOfSelected], (int)SpeedPerMS);
                    }
                }
                Thread.Sleep(10);
                Thread.Sleep(random.Next(300,600));
            }
        }

        public void SetTarget(Being target)
        {
            Target = target;
        }

        public override bool TryGetNearestObject(int distance, out IObjectOnGameMap nearestObject)
        {
            Rectangle pathColider = GetPathColider(distance);
            List<IObjectOnGameMap> objects = new List<IObjectOnGameMap>();
            var selectedItems = from item in NativeRoom.Stuffs
                                where item.Colider.IntersectsWith(pathColider) && item.IsSolid
                                select item;
            if (selectedItems.Count() != 0) objects.AddRange(selectedItems);
            var selectedEnemies = from en in (Target.NativeRoom as DangerousRoom).Enemies
                                  where en.Colider.IntersectsWith(pathColider)
                                  select en;
            if (selectedEnemies.Count() != 0) objects.AddRange(selectedEnemies);
            if (Target.Colider.IntersectsWith(pathColider)) objects.Add(Target);
            if (objects.Count == 0)
            {
                nearestObject = null;
                return false;
            }
            switch (Direction)
            {
                case Directions.Left:
                    var sorted = objects.OrderByDescending(p => p.Colider.Left);
                    nearestObject = sorted.FirstOrDefault();
                    break;
                case Directions.Right:
                    sorted = objects.OrderBy(p => p.Colider.Left);
                    nearestObject = sorted.FirstOrDefault();
                    break;
                case Directions.Up:
                    sorted = objects.OrderByDescending(p => p.Colider.Top);
                    nearestObject = sorted.FirstOrDefault();
                    break;
                case Directions.Down:
                    sorted = objects.OrderBy(p => p.Colider.Top);
                    nearestObject = sorted.FirstOrDefault();
                    break;
                default:
                    throw new Exception();
            }
            return true;
        }

        protected bool AreOnOneLine(out Orientation orientation)
        {
            if (Target.Colider.Top > Colider.Top && Target.Colider.Top < Colider.Bottom ||
                Target.Colider.Bottom > Colider.Top && Target.Colider.Bottom < Colider.Bottom)
            {
                orientation = Orientation.Horizontal;
                return true;
            }
            else if (Target.Colider.Left < Colider.Right && Target.Colider.Left > Colider.Left ||
                Target.Colider.Right > Colider.Left && Target.Colider.Right < Colider.Right)
            {
                orientation = Orientation.Vertical;
                return true;
            }
            else
            {
                orientation = Orientation.Diagonal;
                return false;
            }
        }

        protected Directions GetRelationsWithHeroOnVertical() {
            if (Colider.Top > Target.Colider.Bottom)
                return Directions.Up;
            else
                return Directions.Down;
        }

        protected Directions GetRelationsWithHeroOnHorizontal()
        {
            if (Colider.Left > Target.Colider.Right)
                return Directions.Left;
            else return Directions.Right;
        }
    }
}

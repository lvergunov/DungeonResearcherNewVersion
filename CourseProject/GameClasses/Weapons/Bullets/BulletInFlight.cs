using System.Drawing;
using System.Threading;
using System.Linq;
using GameClasses.GameObjects;
using GameClasses.Game;
using GameClasses.GameMap;
using GameClasses.Beings;
using GameClasses.Enums;
using GameClasses.Beings.Decorators;

namespace GameClasses.Weapons.ShootingWeapons.Bullets
{
    public class BulletInFlight : SimpleBullet, IMovable, IDestroyable
    {
        public IBullet Instance { get; }
        public Being Sender { get; }
        public Rectangle Borders { get; protected set; }
        public Directions Direction { get; protected set; }
        public IWeapon Weapon { get; }
        public BulletInFlight(Being sender,Directions direction,IBullet bullet) : base(bullet.TypeOfBullet, bullet.SpeedPerMS,
            bullet.Rarity, bullet.Place, bullet.Name, bullet.ColliderWidth, bullet.ColliderHeight)
        {
            Instance = bullet;
            Sender = sender;
            Direction = direction;
            Weapon = sender.WeaponInHands;
        }
        public float FullHealth { get; protected set; } = 0.1f;

        public void GetPunched(float damage,Being sender)
        {
            FullHealth = 0;
        }
        public bool IsAlive
        {
            get
            {
                if (FullHealth != 0 && Colider.Left > Sender.NativeRoom.Colider.Left &&
                    Colider.Right < Sender.NativeRoom.Colider.Right && 
                    Colider.Top > Sender.NativeRoom.Colider.Top &&
                    Colider.Bottom < Sender.NativeRoom.Colider.Bottom) return true;
                else return false;
            }
        }
        public void Loose(Room room)
        {
                ChangePosition(new Point(Sender.WeaponInHands.Colider.Left + Sender.WeaponInHands.Colider.Width / 2,
                    Sender.WeaponInHands.Colider.Top + Sender.WeaponInHands.Colider.Height / 2));
            Show();
            while (IsAlive)
            {
                switch (Direction)
                {
                    case Directions.Left:
                            ChangePosition(new Point(Place.X - (int)Instance.SpeedPerMS, Place.Y));
                        break;
                    case Directions.Right:
                            ChangePosition(new Point(Place.X + (int)Instance.SpeedPerMS, Place.Y));
                        break;
                    case Directions.Up:
                            ChangePosition(new Point(Place.X, Place.Y - (int)Instance.SpeedPerMS));
                        break;
                    case Directions.Down:
                            ChangePosition(new Point(Place.X, Place.Y + (int)Instance.SpeedPerMS));
                        break;
                }
                if (room is DangerousRoom dangerousRoom && dangerousRoom.Enemies.Count != 0 &&
                dangerousRoom.Enemies.Any(en => en.IsAlive) && (Sender is Hero || Sender is HeroDecorator))
                {
                    var damagedEnemies = from en in (Sender.NativeRoom as DangerousRoom).Enemies
                                         where Colider.IntersectsWith(en.Colider)
                                         select en;
                    if (damagedEnemies.Count() != 0)
                    {
                        damagedEnemies.First().GetPunched(Weapon.CountDamage(Sender), Sender);
                        Destroy();
                    }
                }
                else if (room is DangerousRoom && Sender is Enemy enemy &&
                    Colider.IntersectsWith(enemy.Target.Colider))
                {
                    enemy.Target.GetPunched(Weapon.CountDamage(Sender),Sender);
                    Destroy();
                }
                else
                {
                    try
                    {
                        var stuffs = from stuff in room.Stuffs
                                     where stuff.Colider.IntersectsWith(Colider)
                                     select stuff;
                        if (stuffs.Count() > 0 && stuffs.First().IsSolid) {
                            Destroy();
                            if (stuffs.First() is IDestroyable destroyable)
                                destroyable.GetPunched(Weapon.CountDamage(Sender),Sender);
                        }
                    }
                    catch { }
                }
                Thread.Sleep(10);
            }
            Destroy();
        }

        public void Destroy()
        {
            FullHealth = 0;
        }
    }
}

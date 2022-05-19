using System;
using System.Drawing;
using GameClasses.Game;
using GameClasses.GameMap;
using GameClasses.Enums;
using GameClasses.Beings;
using System.Threading;

namespace GameClasses.Weapons
{
    public abstract class AbstractWeapon : IWeapon
    {
        public delegate void PunchDelegate(Being owner);
        public Orientation Orientation { get; protected set; }
        public int AbsoluteWidth { get; }
        public int AbsoluteHeight { get; }
        public bool IsVisible { get; protected set; }
        public bool IsSolid { get { return false; } }

        public float Health { get; } = 0;

        public bool IsAlive { get; protected set; } = true;

        public void GetPunched(float damage) {
            IsAlive = false;
        }
        public void Show()
        {
            IsVisible = true;
        }
        public void Hide()
        {
            IsVisible = false;
        }
        public virtual void ChangePosition(Point point)
        {
            Place = point;
        }
        public float CountDamage(Being hero)
        {
            Random random = new Random();
            Thread.Sleep(10);
            float weaponDamage = (float)(random.NextDouble() * (MaxDamage
                - MinDamage) +
                MinDamage);
            Thread.Sleep(10);
            float heroSkill = (float)(random.NextDouble() * (hero.MaximalDamage -
                hero.MinimalDamage) +
                hero.MinimalDamage);
            return weaponDamage + weaponDamage * heroSkill/100;
        }
        public abstract void Use(Being owner);

        public RarityOfStuff Rarity { get; }
        public Rectangle Colider
        {
            get
            {
                return new Rectangle(Place, new Size(ColliderWidth, ColliderHeight));
            }
        }
        public Point Place { get; set; }
        public int ColliderWidth { get; protected set; }
        public int ColliderHeight { get; protected set; }
        public bool IsReloading { get; set; }
        public string Name { get; }
        public virtual float MinDamage { get; }
        public virtual float MaxDamage { get; }
        public virtual int MaxDamageRadius { get; protected set; }
        public virtual int ReloadTime { get; }

        public abstract void SinchroniseDamageRadius(Room room,Directions direction);

        protected AbstractWeapon(RarityOfStuff rarity, Point place, int width, int height,
            string name, float minDamage, float maxDamage, int damageRadius, int reloadTime,
            Orientation orientation = Orientation.Horizontal)
        {
            Rarity = rarity;
            Place = place;
            AbsoluteWidth = width;
            AbsoluteHeight = height;
            if (orientation == Orientation.Horizontal)
            {
                ColliderWidth = width;
                ColliderHeight = height;
            }
            else
            {
                ColliderWidth = height;
                ColliderHeight = width;
            }
            Name = name;
            MaxDamage = maxDamage;
            MaxDamageRadius = damageRadius;
            ReloadTime = reloadTime;
        }
        public int FindEuclideanDistance(IObjectOnGameMap gameObject)
        {
            Point thisObjectCenter = new Point(Colider.Left + Colider.Width, Colider.Top + Colider.Height);
            Point innerObjectCenter = new Point(gameObject.Colider.Left +
                gameObject.Colider.Width, gameObject.Colider.Top + gameObject.Colider.Height);
            return (int)Math.Sqrt(Math.Pow(thisObjectCenter.X - innerObjectCenter.X, 2) +
                Math.Pow(thisObjectCenter.Y - innerObjectCenter.Y, 2));
        }
        public void ChangeOrientation(Orientation orientation)
        {
            if (orientation == Orientation.Vertical)
            {
                ColliderHeight = AbsoluteWidth;
                ColliderWidth = AbsoluteHeight;
            }
            else
            {
                ColliderWidth = AbsoluteWidth;
                ColliderHeight = AbsoluteHeight;
            }
            Orientation = orientation;
        }
        public object Clone() => MemberwiseClone();
    }
}

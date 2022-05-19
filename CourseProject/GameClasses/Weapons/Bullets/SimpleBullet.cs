using System;
using GameClasses.Enums;
using GameClasses.Game;
using System.Drawing;

namespace GameClasses.Weapons.ShootingWeapons.Bullets
{
    public class SimpleBullet : IBullet,ICloneable
    {
        public SimpleBullet(BulletType typeOfBullet, float speedPerMS, 
            RarityOfStuff rarity, Point place, string name, int colliderWidth, 
            int colliderHeight)
        {
            TypeOfBullet = typeOfBullet;
            SpeedPerMS = speedPerMS;
            Rarity = rarity;
            Place = place;
            Name = name;
            ColliderWidth = colliderWidth;
            ColliderHeight = colliderHeight;
        }

        public BulletType TypeOfBullet { get; }
        public bool IsSolid { get; protected set; } = false;
        public float SpeedPerMS { get; protected set; }

        public RarityOfStuff Rarity { get; }

        public Point Place { get; protected set; }

        public Rectangle Colider
        {
            get {
                return new Rectangle(Place,new Size(ColliderWidth,ColliderHeight));
            }
        }

        public bool IsVisible { get; protected set; } = false;

        public string Name { get; }

        public int ColliderWidth { get; }

        public int ColliderHeight { get; }

        public void ChangePosition(Point newPoint)
        {
            Place = newPoint;
        }

        public object Clone() => MemberwiseClone();

        public int FindEuclideanDistance(IObjectOnGameMap gameObject)
        {
            Point thisObjectCenter = new Point(Colider.Left + Colider.Width, Colider.Top + Colider.Height);
            Point innerObjectCenter = new Point(gameObject.Colider.Left +
                gameObject.Colider.Width, gameObject.Colider.Top + gameObject.Colider.Height);
            return (int)Math.Sqrt(Math.Pow(thisObjectCenter.X - innerObjectCenter.X, 2) +
                Math.Pow(thisObjectCenter.Y - innerObjectCenter.Y, 2));
        }

        public void Hide()
        {
            IsVisible = false;
        }

        public void Show()
        {
            IsVisible = true;
        }
    }
}

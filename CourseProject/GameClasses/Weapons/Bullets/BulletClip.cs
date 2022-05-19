using System;
using System.Linq;
using System.Collections.Generic;
using GameClasses.Enums;
using GameClasses.Game;
using GameClasses.GameObjects;
using System.Drawing;
using GameClasses.Weapons.ShootingWeapons.Bullets;
using GameClasses.Beings;
using GameClasses.Weapons.ShootingWeapons;

namespace GameClasses.Weapons.Bullets
{
    public class BulletClip : IGenerable,IDestroyable
    {
        public RarityOfStuff Rarity { get; }

        public Point Place { get; protected set; }

        public float FullHealth { get {
                return (float)Bullets.Count;
            } }

        public void GetPunched(float d,Being s) { }

        public bool IsAlive
        {
            get
            {
                if (FullHealth > 0) return true;
                else return false;
            }
        }

        public Rectangle Colider { get {
                return new Rectangle(Place,new Size(ColliderWidth,ColliderHeight));
            } }

        public Rectangle ColiderOfInteraction
        {
            get {
                return new Rectangle(Place.X-RadiusOfInteraction,Place.Y-RadiusOfInteraction,
                    ColliderWidth+2*RadiusOfInteraction,ColliderHeight+2*RadiusOfInteraction);
            }
        }

        public string Description { get {
                return Name + $" of {TypeOfBullet.ToString()} inside. Number: {Bullets.Count}";
            } }

        public void Use(Hero hero)
        {
            foreach (IWeapon weapon in hero.Arsenal) { 
                if(weapon!=null && weapon is ShootingWeapon shootingWeapon)
                {
                    while (shootingWeapon.TryGiveAmmo(Bullets[0]) && IsAlive)
                        Bullets.RemoveAt(0);
                }
            }
        }

        public int RadiusOfInteraction { get; }

        public List<IBullet> Bullets { get; }

        public BulletType TypeOfBullet { get; }

        public BulletClip(List<IBullet> bullets,BulletType bulletType,Point place,int coliderWidth,
            int coliderHeight) {
            if (bullets.Any(b => b.TypeOfBullet != bulletType))
                throw new Exception();
            else {
                Bullets = bullets;
                TypeOfBullet = bulletType;
                ColliderWidth = coliderWidth;
                ColliderHeight = coliderHeight;
                RadiusOfInteraction = 10;
            }
        }

        public bool IsVisible { get; protected set; }

        public bool IsSolid { get; } = false;

        public string Name { get; }

        public int ColliderWidth { get; }

        public int ColliderHeight { get; }

        public void ChangePosition(Point newPoint)
        {
            Place = newPoint;
        }

        public int FindEuclideanDistance(IObjectOnGameMap onGameMap)
        {
            Point thisObjectCenter = new Point(Colider.Left + Colider.Width, Colider.Top + Colider.Height);
            Point innerObjectCenter = new Point(onGameMap.Colider.Left +
                onGameMap.Colider.Width, onGameMap.Colider.Top + onGameMap.Colider.Height);
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

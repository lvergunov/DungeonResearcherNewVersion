using GameClasses.Beings;
using GameClasses.Enums;
using GameClasses.Game;
using GameClasses.Weapons;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClasses.GameObjects
{
    public class WeaponInChest : IInteractiveObject,IGenerable,IObjectOnGameMap,IDestroyable
    {
        public IWeapon WeaponInside { get; }

        public float FullHealth { get; protected set; } = 1;

        public void GetPunched(float damage, Being sender) { }

        public bool IsAlive { get {
                if (FullHealth > 0) return true;
                else return false;
            } }
        public WeaponInChest(IWeapon weapon) {
            RadiusOfInteraction = 10;
            WeaponInside = weapon;
            Place = weapon.Place;
            ColliderWidth = weapon.ColliderWidth;
            ColliderHeight = weapon.ColliderHeight;
            Rarity = weapon.Rarity;
        }

        public int RadiusOfInteraction { get; }

        public Rectangle ColiderOfInteraction {
            get
            {
                return new Rectangle(Place.X - RadiusOfInteraction, Place.Y - RadiusOfInteraction,
                    ColliderWidth + 2 * RadiusOfInteraction, ColliderHeight + 2 * RadiusOfInteraction);
            }
        }

        public string Description { get {
                return WeaponInside.Name;
            } }

        public Point Place { get; protected set; }

        public Rectangle Colider
        {
            get { return new Rectangle(Place,new Size(ColliderWidth,ColliderHeight)); }
        }
        public bool IsVisible { get; protected set; }

        public bool IsSolid { get; } = false;

        public string Name { get { return WeaponInside.Name; } }

        public int ColliderWidth { get; }

        public int ColliderHeight { get; }

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

        public void Use(Hero hero)
        {
            hero.TakeNewWeapon(WeaponInside);
            FullHealth = 0;
        }

        public void ChangePosition(Point newPosition) {
            Place = newPosition;
        }

        public RarityOfStuff Rarity { get; } = RarityOfStuff.Casual;
    }
}

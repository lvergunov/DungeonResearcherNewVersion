using System;
using System.Drawing;
using GameClasses.Beings;
using GameClasses.Enums;
using GameClasses.Game;

namespace GameClasses.GameObjects.Poisons
{
    public class HealthPoison : IGenerable, IInteractiveObject, IDestroyable
    {
        public HealthPoison(PoisonType poisonType, RarityOfStuff rarity, Point place, string name, int colliderWidth,
            int colliderHeight, float healthRestoringRate)
        {
            Rarity = rarity;
            Place = place;
            Name = name;
            ColliderWidth = colliderWidth;
            ColliderHeight = colliderHeight;
            HealthRestoringRate = healthRestoringRate;
            TypeOfPoison = poisonType;
            RadiusOfInteraction = 10;
        }

        public float FullHealth
        {
            get; private set;
        } = 1;

        public void GetPunched(float d, Being s) {
        }

        public bool IsAlive
        {
            get {
                if (FullHealth > 0) return true;
                else return false;
            }
        }

        public int RadiusOfInteraction { get; }

        public Rectangle ColiderOfInteraction
        {
            get
            {
                return new Rectangle(Place.X - RadiusOfInteraction, Place.Y - RadiusOfInteraction,
                    ColliderWidth + 2 * RadiusOfInteraction, ColliderHeight + 2 * RadiusOfInteraction);
            }
        }

        public string Description
        {
            get
            {
                return Name + $" restores {HealthRestoringRate} HP.";
            }
        }

        public void Use(Hero hero)
        {
            hero.RestoreHealth(HealthRestoringRate);
            FullHealth = 0;
        }

        public PoisonType TypeOfPoison { get; }

        public RarityOfStuff Rarity { get; }

        public Point Place { get; protected set; }

        public Rectangle Colider
        {
            get
            {
                return new Rectangle(Place, new Size(ColliderWidth,ColliderHeight));
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

        public float HealthRestoringRate { get; }

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

using System;
using System.Drawing;
using GameClasses.Game;
using GameClasses.Enums;

namespace GameClasses.GameObjects.LetsAndTraps
{
    public abstract class BaseBlock : IObjectOnGameMap,IGenerable
    {
        public Rectangle Colider
        {
            get
            {
                return new Rectangle(Place.X, Place.Y, ColliderWidth, ColliderHeight);
            }
        }
        public Point Place { get; protected set; }

        public void ChangePosition(Point newPlace)
        {
            Place = newPlace;
        }

        public bool IsVisible { get; protected set; }
        public void Show() { IsVisible = true; }
        public void Hide() { IsVisible = false; }

        public Rectangle BaseForm { get; }

        public string Name { get; }

        public int ColliderWidth { get { return BaseForm.Width; } }
        public int ColliderHeight { get { return BaseForm.Height; } }

        public bool IsSolid { get; protected set; }

        public BaseBlock(Point location, int side)
        {
            BaseForm = new Rectangle(location, new Size(side, side));
        }
        public int FindEuclideanDistance(IObjectOnGameMap gameObject)
        {
            Point thisObjectCenter = new Point(Colider.Left + Colider.Width, Colider.Top + Colider.Height);
            Point innerObjectCenter = new Point(gameObject.Colider.Left +
                gameObject.Colider.Width, gameObject.Colider.Top + gameObject.Colider.Height);
            return (int)Math.Sqrt(Math.Pow(thisObjectCenter.X - innerObjectCenter.X, 2) +
                Math.Pow(thisObjectCenter.Y - innerObjectCenter.Y, 2));
        }

        public RarityOfStuff Rarity { get; }
    }
}

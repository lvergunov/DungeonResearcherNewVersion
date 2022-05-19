using System;
using System.Drawing;
using GameClasses.Game;
using GameClasses.Enums;

namespace GameClasses.GameMap
{
    public abstract class Insertion : IObjectOnGameMap
    {
        public int Number { get; }

        public string Name { get { return $"Insertion {Number}"; } }

        public int ColliderWidth { get; }

        public int ColliderHeight { get; }

        public Point Place { get; protected set; }
        public bool IsVisible { get; protected set; }

        public bool IsDestroyable { get; } = false;

        public bool IsSolid { get; } = false;

        public abstract void PlaceComponentsOnMap();
        public void Show()
        {
            IsVisible = true;
        }
        public void Hide()
        {
            IsVisible = false;
        }
        public Rectangle Colider {
            get
            {
                return new Rectangle(Place.X, Place.Y, ColliderWidth, ColliderHeight);
            }
        }
        public Insertion(Point leftTopPoint,int width,int height)
        {
            Place = leftTopPoint;
            ColliderWidth = width;
            ColliderHeight = height;
            Show();
        }
        public int FindEuclideanDistance(IObjectOnGameMap gameObject)
        {
            Point thisObjectCenter = new Point(Colider.Left+Colider.Width,Colider.Top+Colider.Height);
            Point innerObjectCenter = new Point(gameObject.Colider.Left + 
                gameObject.Colider.Width, gameObject.Colider.Top + gameObject.Colider.Height);
            return (int)Math.Sqrt(Math.Pow(thisObjectCenter.X-innerObjectCenter.X,2)+
                Math.Pow(thisObjectCenter.Y - innerObjectCenter.Y,2));
        }
    }
}

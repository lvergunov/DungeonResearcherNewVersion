using GameClasses.Game;
using GameClasses.Enums;
using System;
using System.Drawing;

namespace GameClasses.GameMap
{
    public class LevelExit : IObjectOnGameMap
    {
        public RarityOfStuff Rarity { get; }
        public Point Place { get; }
        public Insertion Room { get; }
        public Rectangle Colider { get {
                return new Rectangle(Place, new Size(ColliderWidth,ColliderHeight));  
            } 
        }

        public bool IsDestroyable { get; } = false;

        public bool IsSolid { get; } = false;

        public LevelExit(Point position,
            Insertion room, int colliderWidth = 30, int colliderHeight = 30)
        {
            Place = position;
            ColliderWidth = colliderWidth;
            ColliderHeight = colliderHeight;
            Room = room;
            Show();
        }
        public bool IsVisible { get; private set; }

        public string Name { get; }

        public int ColliderWidth { get; }

        public int ColliderHeight { get; }

        public void Hide()
        {
            IsVisible = false;
        }

        public void Show()
        {
            IsVisible = true;
        }
        public int FindEuclideanDistance(IObjectOnGameMap gameObject)
        {
            Point thisObjectCenter = new Point(Colider.Left + Colider.Width, Colider.Top + Colider.Height);
            Point innerObjectCenter = new Point(gameObject.Colider.Left +
                gameObject.Colider.Width, gameObject.Colider.Top + gameObject.Colider.Height);
            return (int)Math.Sqrt(Math.Pow(thisObjectCenter.X - innerObjectCenter.X, 2) +
                Math.Pow(thisObjectCenter.Y - innerObjectCenter.Y, 2));
        }
    }
}

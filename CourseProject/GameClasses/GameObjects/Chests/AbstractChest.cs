using GameClasses.Enums;
using GameClasses.Game;
using GameClasses.Factory.StuffFactory;
using System;
using System.Drawing;
using GameClasses.Beings;

namespace GameClasses.GameObjects.Chests
{
    public class AbstractChest : IGenerable,IInteractiveObject, IDestroyable
    {
        public delegate void OpenActionsDelegate(AbstractChest chest);
        
        public event OpenActionsDelegate onOpen;

        public int RadiusOfInteraction { get; protected set; } = 10;

        public bool IsOpened { get; protected set; }



        public Rectangle ColiderOfInteraction { get {
                return new Rectangle(Place.X - RadiusOfInteraction,Place.Y - RadiusOfInteraction,
                    ColliderWidth + 2*RadiusOfInteraction,ColliderHeight + 2*RadiusOfInteraction);
            } 
        }

        public AbstractChest(Point place,RarityOfStuff rarity,string name,int coliderWidth,int coliderHeight) {
            Place = place;
            Rarity = rarity;
            Name = name;
            ColliderWidth = coliderWidth;
            ColliderHeight = coliderHeight;
            InsideObject = new ChestStuffFactory().GetGameObject(place);
            InsideObject.Hide();
            this.Show();
        }

        public void ChangePosition(Point newPoint)
        {
            Place = newPoint;
        }

        public void Open()
        {
            IsOpened = true;
            onOpen(this);
            this.Hide();
            InsideObject.ChangePosition(Place);
            InsideObject.Show();
            FullHealth = 0;
        }

        public IGenerable InsideObject { get; }

        public Point Place { get; protected set; }

        public Rectangle Colider
        {
            get { return new Rectangle(Place, new Size(ColliderWidth, ColliderHeight)); }
        }

        public bool IsVisible { get; protected set; }

        public bool IsSolid { get { return true; } }

        public RarityOfStuff Rarity { get; }

        public string Name { get; }

        public int ColliderWidth { get; }

        public int ColliderHeight { get; }

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

        public string Description { get {
                return Name + " with one item inside.";
            } }

        public float FullHealth
        {
            get; protected set;
        } = 1;

        public bool IsAlive {
            get {
                if (FullHealth > 0)
                    return true;
                else return false;
            }
        }

        public void Use(Hero hero)
        {
            Open();
            hero.NativeRoom.Add(InsideObject);
        }

        public void GetPunched(float damageRate, Being sender)
        {
        }
    }
}

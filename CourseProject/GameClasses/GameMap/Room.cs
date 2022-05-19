using System;
using System.Linq;
using System.Threading;
using System.Drawing;
using System.Collections.Generic;
using GameClasses.Game;
using GameClasses.Factory;
using GameClasses.GameObjects;
using GameClasses.GameObjects.Chests;
using GameClasses.Enums;
using GameClasses.Factory.StuffFactory;

namespace GameClasses.GameMap
{
    public class Room : Insertion
    {
        public List<IGenerable> Stuffs { get; protected set; }
        public List<Rectangle> Grid { get; protected set; }
        public Room(Point leftTopPoint, int width, int height) : base(leftTopPoint, width,height)
        {
            Stuffs = new List<IGenerable>();
        }
        public override void PlaceComponentsOnMap()
        {
            SetComponentsOnMap(GetStuff());
        }
        protected void SetComponentsOnMap(List<IGenerable> components)
        {
            Grid = new List<Rectangle>();
            int biggestWidth = components.Max(e => e.ColliderWidth);
            int biggestHeight = components.Max(e => e.ColliderHeight);
            if (Colider.Width / biggestWidth < 2 || Colider.Height / biggestHeight < 2)
                throw new Exception();
            int widthLeft = Colider.Width;
            List<Rectangle> widthParts = new List<Rectangle>();
            while (widthLeft >= biggestWidth)
            {
                widthParts.Add(new Rectangle(Colider.Right - widthLeft, Colider.Top, biggestWidth, Colider.Height));
                widthLeft -= biggestWidth;
            }
            foreach (Rectangle part in widthParts)
            {
                int heightLeft = Colider.Height;
                while (heightLeft >= biggestHeight)
                {
                    Grid.Add(new Rectangle(part.Left, Colider.Bottom - heightLeft,
                        part.Width, biggestHeight));
                    heightLeft -= biggestHeight;
                }
            }
            if ((float)components.Count / Grid.Count > 0.8)
                throw new Exception("Too big accuracy of enemies");
            else
            {
                foreach (IGenerable component in components)
                {
                    Thread.Sleep(10);
                    int numberOfGrid = random.Next(0, Grid.Count - 1);
                    Rectangle squareToSpawn = Grid[numberOfGrid];
                    component.ChangePosition(squareToSpawn.Location);
                    Grid.Remove(squareToSpawn);
                    Stuffs.Add(component);
                }
                Grid.Clear();
            }
        }
        protected Random random = new Random();
        public void Add(IGenerable obj) {
            Stuffs.Add(obj);
        }
        protected List<IGenerable> GetStuff() {
            List<IGenerable> totalListOfComponents = new List<IGenerable>();
            Thread.Sleep(10);
            int numberOfChests = random.Next(0, 2);
            for (int i = 0; i < numberOfChests; i++)
                totalListOfComponents.Add(new AbstractChest(Point.Empty,RarityOfStuff.Casual,"Chest",
                    20,20));
            Thread.Sleep(10);
            int numberOfLets = random.Next(5,8);
            for (int i = 0; i < numberOfLets; i++)
                totalListOfComponents.Add(new LetFactory().Create(Point.Empty,10));
            return totalListOfComponents;
        }
    }
}

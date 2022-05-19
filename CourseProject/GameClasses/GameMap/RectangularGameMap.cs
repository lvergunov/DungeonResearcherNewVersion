using System;
using System.Threading;
using System.Drawing;
using System.Collections.Generic;
using GameClasses.Enums;
using GameClasses.ProcedureGeneration;
using GameClasses.Weapons;

namespace GameClasses.GameMap
{
    public class RectangularGameMap : IRectangularMap
    {
        public RectangularGameMap RootMap { get; }
        public Leaf ChildNodeOne { get; protected set; }
        public Leaf ChildNodeTwo { get; protected set; }
        public Rectangle Form { get; protected set; }
        public int LevelNumber { get; }
        public bool IsBossLevel { get; }
        public List<Insertion> Insertions { get; private set; }

        public void Add(Insertion insertion)
        {
            Insertions.Add(insertion);
        }
        public List<RoomExit> Coridors { get; private set; }
        public LevelExit Exit { get; protected set; }
        public void Add(RoomExit exit)
        {
            Coridors.Add(exit);
        }
        public void Add(LevelExit levelExit)
        {
            Exit = levelExit;
        }
        public static List<Leaf> Leaves { get; protected set; }
        public void Add(Leaf leaf)
        {
            Leaves.Add(leaf);
        }

        public RectangularGameMap(int levelNumber,int lastIteration, int width, int height, Orientation devidingDir,
            bool isBossLevel = false)
        {
            Form = new Rectangle(0,0, width, height);
            LevelNumber = levelNumber;
            IsBossLevel = isBossLevel;
            Insertions = new List<Insertion>();
            Coridors = new List<RoomExit>();
            Leaves = new List<Leaf>();
            RootMap = this;
            CreateLeaves(devidingDir,lastIteration,0);
        }
        protected RectangularGameMap()
        {
        }
        protected void CreateLeaves(Orientation devidingDirection,int lastIteration,int thisIteration)
        {
            if (devidingDirection == Orientation.Vertical)
            {
                int randomiserLeft = Form.Left + Form.Width / 3;
                int randomiserRight = Form.Right - Form.Width / 3;
                Thread.Sleep(10);
                int devidingPoint = randomiser.Next(randomiserLeft,randomiserRight);
                Rectangle leftLeaf = new Rectangle(Form.Location,new Size(devidingPoint - Form.X,Form.Height));
                Rectangle rightLeaf = new Rectangle(devidingPoint,Form.Y,Form.Right - devidingPoint,Form.Height);
                thisIteration++;
                ChildNodeOne = new Leaf(this, leftLeaf,devidingDirection, lastIteration, thisIteration);
                ChildNodeTwo = new Leaf(this, rightLeaf,devidingDirection, lastIteration, thisIteration);
            }
            else
            {
                int randomiserTop = Form.Top + Form.Height / 3;
                int randomiserBottom = Form.Bottom - Form.Height / 3;
                Thread.Sleep(10);
                int devidingPoint = randomiser.Next(randomiserTop,randomiserBottom);
                Rectangle topLeaf = new Rectangle(Form.Location,new Size(Form.Width,devidingPoint-Form.Top));
                Rectangle bottomLeaf = new Rectangle(Form.X,devidingPoint,Form.Width,Form.Bottom-devidingPoint);
                thisIteration++;
                ChildNodeOne = new Leaf(this,topLeaf,devidingDirection,lastIteration,thisIteration);
                ChildNodeTwo = new Leaf(this, bottomLeaf, devidingDirection,lastIteration, thisIteration);
            }
        }
        protected Random randomiser = new Random();
    }
}

using System.Drawing;
using GameClasses.Enums;
using GameClasses.Game;
using GameClasses.GameMap;
using System;

namespace GameClasses.GameMap
{
    public class RoomExit : IObjectOnGameMap
    {
        public RarityOfStuff Rarity { get; }
        public Directions SideOfRoom { get; }
        public Point Place { get { return Colider.Location; } }
        public string Name { get; }
        public void ChangePosition(Point newPlace) { }
        public bool IsVisible { get; protected set; }
        public bool IsSolid { get; } = false;
        public bool IsDestroyable { get; } = false;
        public void Show() { IsVisible = true; }
        public void Hide() { IsVisible = false; }
        public bool IsOpened { get; protected set; }
        public int ColliderWidth { get { return Colider.Width; } }
        public int ColliderHeight { get { return Colider.Height; } }
        public Room OuterRoom { get; }
        public Room InnerRoom { get; }
        public Rectangle Colider { get; }
        public RoomExit() { }
        public RoomExit(Room outerRoom, Room innerRoom, int doorWidth = 30, int doorHeight = 5)
        {
            OuterRoom = outerRoom;
            InnerRoom = innerRoom;
            IsOpened = true;
            Orientation? orientation;
            Random randomiser = new Random();
            orientation = CheckRelations(InnerRoom.Colider, OuterRoom.Colider);
            if (orientation == Orientation.Vertical && OuterRoom.Colider.Y < InnerRoom.Colider.Y)
            {
                SideOfRoom = Directions.Up;
                int outerEnterPosX = randomiser.Next(InnerRoom.Colider.Left + InnerRoom.Colider.Width / 3,
                        InnerRoom.Colider.Right - InnerRoom.Colider.Width / 3);
                Colider = new Rectangle(outerEnterPosX, InnerRoom.Colider.Top, doorWidth, doorHeight);
            }
            else if (orientation == Orientation.Vertical && OuterRoom.Colider.Y > InnerRoom.Colider.Y)
            {
                SideOfRoom = Directions.Down;
                int outerEnterPosX = randomiser.Next(InnerRoom.Colider.Left + InnerRoom.Colider.Width / 3,
                    InnerRoom.Colider.Right - InnerRoom.Colider.Width / 3);
                Colider = new Rectangle(outerEnterPosX, InnerRoom.Colider.Bottom - doorHeight, doorWidth, doorHeight);
            }
            else if (orientation == Orientation.Horizontal && OuterRoom.Colider.Right < InnerRoom.Colider.Left)
            {
                SideOfRoom = Directions.Left;
                int outerEnterPosY = randomiser.Next(InnerRoom.Colider.Top + InnerRoom.Colider.Height / 3,
                    InnerRoom.Colider.Bottom - InnerRoom.Colider.Height / 3);
                Colider = new Rectangle(InnerRoom.Colider.Left, outerEnterPosY, doorHeight, doorWidth);
            }
            else if (orientation == Orientation.Horizontal && InnerRoom.Colider.Right < OuterRoom.Colider.Left)
            {
                SideOfRoom = Directions.Right;
                int outerEnterPosY = randomiser.Next(InnerRoom.Colider.Top + InnerRoom.Colider.Height / 3,
                    InnerRoom.Colider.Bottom - InnerRoom.Colider.Height / 3);
                Colider = new Rectangle(InnerRoom.Colider.Right - doorHeight, outerEnterPosY, doorHeight, doorWidth);
            }
            Show();
        }

        public static Orientation? CheckRelations(Rectangle roomOne, Rectangle roomTwo)
        {
            if ((roomTwo.Bottom<=roomOne.Top || roomOne.Bottom<=roomTwo.Top) 
                && (roomOne.Left>=roomTwo.Left || roomTwo.Left>=roomOne.Left))
                return Orientation.Vertical;
            else if ((roomTwo.Right<=roomOne.Left || roomOne.Right<=roomTwo.Left) 
                && (roomOne.Top>=roomTwo.Top || roomTwo.Top>=roomOne.Top)) 
                return Orientation.Horizontal;
            else return null;
        }

        public bool IsOuterTop(out bool IsWrapped)
        {
            if (OuterRoom.Colider.Top < InnerRoom.Colider.Top && OuterRoom.Colider.Bottom < InnerRoom.Colider.Bottom)
            {
                IsWrapped = false;
                return true;
            }
            if (OuterRoom.Colider.Top < InnerRoom.Colider.Top && OuterRoom.Colider.Bottom < InnerRoom.Colider.Bottom)
            {
                IsWrapped = true;
                return true;
            }
            if (OuterRoom.Colider.Top > InnerRoom.Colider.Top && OuterRoom.Colider.Bottom < InnerRoom.Colider.Bottom)
            {
                IsWrapped = true;
                return false;
            }
            else
            {
                IsWrapped = false;
                return false;
            }
        }

        public bool IsOuterLeft(out bool IsWrapped)
        {
            if (OuterRoom.Colider.Left < InnerRoom.Colider.Left && OuterRoom.Colider.Right < InnerRoom.Colider.Right)
            {
                IsWrapped = false;
                return true;
            }
            if (OuterRoom.Colider.Left < InnerRoom.Colider.Left && OuterRoom.Colider.Right > InnerRoom.Colider.Right)
            {
                IsWrapped = true;
                return true;
            }
            if (OuterRoom.Colider.Left > InnerRoom.Colider.Left && OuterRoom.Colider.Right < InnerRoom.Colider.Right)
            {
                IsWrapped = true;
                return false;
            }
            else
            {
                IsWrapped = false;
                return false;
            }
        }
        public void OpenExit()
        {
            IsOpened = true;
        }
        public void CloseExit()
        {
            IsOpened = false;
        }
        public override bool Equals(object obj)
        {
            if (obj is RoomExit exit)
            {
                return this.OuterRoom == exit.InnerRoom && this.InnerRoom == exit.OuterRoom ||
                    this.OuterRoom == exit.OuterRoom && this.InnerRoom == exit.OuterRoom;
            }
            else return false;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
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

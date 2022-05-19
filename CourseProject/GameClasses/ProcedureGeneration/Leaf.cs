using System.Threading;
using GameClasses.GameMap;
using GameClasses.Enums;
using System.Drawing;

namespace GameClasses.ProcedureGeneration
{
    public class Leaf : RectangularGameMap
    {
        public int NodeGeneration { get; }
        public RectangularGameMap ParentalLeaf { get; }
        public Insertion InnerInsertion { get; protected set; }
        public Leaf(RectangularGameMap parentalLeaf,Rectangle baseForm,Orientation lastDevide,int finalIter,int numberOfIter) : base()
        {
            ParentalLeaf = parentalLeaf;
            NodeGeneration = numberOfIter;
            Form = baseForm;
            Leaves.Add(this);
            if (finalIter == numberOfIter)
                CreateRoom();
            else
            {
                if(lastDevide == Orientation.Horizontal)
                    CreateLeaves(Orientation.Vertical,finalIter,numberOfIter);
                else CreateLeaves(Orientation.Horizontal, finalIter, numberOfIter);
            }
        }
        protected void CreateRoom() {
            Thread.Sleep(10);
            Point basePoint = new Point(randomiser.Next(Form.Left,Form.Left+Form.Width/3),
                randomiser.Next(Form.Top,Form.Top+Form.Height/3));
            Thread.Sleep(10);
            int roomHeight = randomiser.Next(Form.Bottom - Form.Height/2,Form.Bottom) - basePoint.Y;
            Thread.Sleep(10);
            int roomWidth = randomiser.Next(Form.Right - Form.Width/2,Form.Right) - basePoint.X;
            InnerInsertion = new Room(basePoint,roomWidth,roomHeight);
        }
    }
}

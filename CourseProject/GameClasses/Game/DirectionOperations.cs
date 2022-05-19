using System;
using GameClasses.Enums;

namespace GameClasses.Game
{
    public class DirectionOperatons
    {
        public static Directions GetOpposite(Directions direction) {
            switch (direction)
            {
                case Directions.Left:
                    return Directions.Right;
                case Directions.Up:
                    return Directions.Down;
                case Directions.Right:
                    return Directions.Left;
                case Directions.Down:
                    return Directions.Up;
                default:
                    throw new Exception();
            }
        }
    }
}

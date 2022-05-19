using GameClasses.GameObjects;
using System;
using System.Drawing;
using System.Threading;
using System.Collections.Generic;
using GameClasses.GameObjects.LetsAndTraps;
using GameClasses.Enums;

namespace GameClasses.Factory.StuffFactory
{
    public class LetFactory : LetsAbstractFactory
    {
        protected delegate BaseBlock CreationDelegate(Point place,int side);
        protected List<CreationDelegate> methods = new List<CreationDelegate>();

        public LetFactory()
        {
            methods.AddRange(new CreationDelegate[] { GetWall,GetTrap,GetPit});
        }
        public override IGenerable Create(Point place,int size)
        {
            Random random = new Random();
            Thread.Sleep(10);
            int number = random.Next(0,methods.Count);
            return methods[number].Invoke(place,size);
        }

        public BaseBlock GetWall(Point place,int size) {
            return new BlockOfWall(place,size);
        }

        public BaseBlock GetPit(Point place,int size)
        {
            return new BlockOfPit(place,size);
        }

        public BaseBlock GetTrap(Point place,int size)
        {
            return new BlockOfTrap(TrapType.SimpleTrap,place,size,5000,5);
        }
    }
}

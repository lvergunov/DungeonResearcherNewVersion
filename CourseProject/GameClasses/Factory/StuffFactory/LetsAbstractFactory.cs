using System.Drawing;
using GameClasses.GameObjects;

namespace GameClasses.Factory.StuffFactory
{
    public abstract class LetsAbstractFactory
    {
        public abstract IGenerable Create(Point place,int side); 
    }
}

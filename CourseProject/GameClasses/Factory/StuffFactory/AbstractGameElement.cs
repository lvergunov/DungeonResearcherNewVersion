using System.Drawing;
using GameClasses.GameObjects;

namespace GameClasses.Factory.StuffFactory
{
    public abstract class AbstractGameElementFactory
    {
        public abstract IGenerable GetGameObject(Point place);
    }
}

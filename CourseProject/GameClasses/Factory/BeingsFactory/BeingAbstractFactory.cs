using System.Drawing;
using System.Collections.Generic;
using GameClasses.Beings;
using GameClasses.GameMap;
using GameClasses.Weapons;

namespace GameClasses.Factory.BeingsFactory
{
    public abstract class BeingAbstractFactory
    {
        public abstract Enemy Create(Room roomToSpawn, IWeapon weapon, Point point);
    }
}

using GameClasses.Weapons;
using GameClasses.Enums;
using System.Drawing;

namespace GameClasses.Factory.StuffFactory
{
    public abstract class WeaponAbstractFactory
    {
        public abstract IWeapon CreateStuff(Point point);
    }
}

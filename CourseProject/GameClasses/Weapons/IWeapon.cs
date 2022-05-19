using System;
using GameClasses.GameObjects;
using GameClasses.GameMap;
using GameClasses.Enums;
using GameClasses.Beings;

namespace GameClasses.Weapons
{
    public interface IWeapon : IGenerable,ICloneable
    {
        Orientation Orientation { get; }
        bool IsReloading { get; }
        void Use(Being owner);
        float MaxDamage { get; }
        float MinDamage { get; }
        int MaxDamageRadius { get; }
        int ReloadTime { get; }
        int AbsoluteWidth { get; }
        int AbsoluteHeight { get; }
        void ChangeOrientation(Orientation orientation);
        void SinchroniseDamageRadius(Room room,Directions direction);
        float CountDamage(Being hero);
    }
}

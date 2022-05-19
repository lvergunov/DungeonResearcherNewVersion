using System;
using GameClasses.Game;
using GameClasses.GameObjects;
using GameClasses.Enums;
using System.Drawing;
using System.Threading.Tasks;
using System.Threading;
using GameClasses.GameMap;
using GameClasses.Beings;

namespace GameClasses.Weapons.ColdWeapons
{
    public class ColdWeapon : AbstractWeapon
    {
        public event PunchDelegate onColdWeaponPunch;
        public ColdWeapon(ColdWeaponType type,RarityOfStuff rarity, Point place, int width, int height,
            string name, float minDamage, float maxDamage, int damageRadius, int reloadTime,
            Orientation orientation = Orientation.Horizontal) : base(rarity,place,width,height,
                name,minDamage,maxDamage,damageRadius,reloadTime,orientation)
        {
            TypeOfWeapon = type;
        }
        public ColdWeaponType TypeOfWeapon { get;}
        public override async void Use(Being owner)
        {
            onColdWeaponPunch(owner);
            IsReloading = true;
            await Task.Run(() =>
            {
                Thread.Sleep(ReloadTime);
                IsReloading = false;
            });
        }
        public override void SinchroniseDamageRadius(Room room, Directions direction)
        {
        }
    }
}

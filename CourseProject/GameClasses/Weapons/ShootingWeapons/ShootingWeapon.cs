using System;
using System.Drawing;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GameClasses.Enums;
using GameClasses.GameMap;
using GameClasses.Beings;
using GameClasses.Weapons.ShootingWeapons.Bullets;

namespace GameClasses.Weapons.ShootingWeapons
{
    public class ShootingWeapon : AbstractWeapon
    {
        public event PunchDelegate onShootingPunch;
        public List<IBullet> Bullets { get; protected set; }
        public IBullet ReadyBullet { get; protected set; } 
        public BulletType TypeOfBullet { get; }
        public int BulletMaximum { get; }

        public ShootingWeapon(ShootingWeaponType weaponType,BulletType typeOfBullet,RarityOfStuff rarity, Point place, int width, int height,
             string name, float minDamage, float maxDamage, int damageRadius, int reloadTime, int bulletMaximum,
             List<IBullet> bullets,Orientation orientation = Orientation.Horizontal) :
            base(rarity, place, width, height, name, minDamage, maxDamage, damageRadius,
                reloadTime, orientation)
        {
            TypeOfBullet = typeOfBullet;
            BulletMaximum = bulletMaximum;
            TypeOfWeapon = weaponType;
            if (bullets.Any(b => b.TypeOfBullet != TypeOfBullet))
                throw new Exception("Incorrect type of bullets");
            Bullets = bullets;
        }

        public ShootingWeaponType TypeOfWeapon { get; }

        public async override void Use(Being owner)
        {
            if (Bullets.Count != 0 && !IsReloading)
            {
                ReadyBullet = Bullets[0];
                Bullets.Remove(ReadyBullet);
                onShootingPunch(owner);
                IsReloading = true;
                await Task.Run(() =>
                {
                    Thread.Sleep(ReloadTime);
                    IsReloading = false;
                });
            }
        }

        public override void ChangePosition(Point place) {
            Place = place;
            Bullets.ForEach(b=>b.ChangePosition(place));
        }

        public override void SinchroniseDamageRadius(Room room,Directions direction) {
            switch (direction)
            {
                case Directions.Up:
                    MaxDamageRadius = Colider.Bottom - room.Colider.Top;
                    break;
                case Directions.Down:
                    MaxDamageRadius = room.Colider.Bottom - Colider.Top;
                    break;
                case Directions.Left:
                    MaxDamageRadius = Colider.Right - room.Colider.Left;
                    break;
                case Directions.Right:
                    MaxDamageRadius = room.Colider.Right - Colider.Left;
                    break;
            }
        }

        public bool TryGiveAmmo(IBullet bullet)
        {
            if (bullet.TypeOfBullet == TypeOfBullet && Bullets.Count < BulletMaximum)
            {
                Bullets.Add(bullet);
                return true;
            }
            else return false;
        }
    }
}

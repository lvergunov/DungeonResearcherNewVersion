using GameClasses.Weapons;
using GameClasses.Enums;
using GameClasses.Weapons.ColdWeapons;
using GameClasses.Weapons.ShootingWeapons;
using GameClasses.Weapons.ShootingWeapons.Bullets;
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Threading;

namespace GameClasses.Factory.StuffFactory
{
    public class WeaponFactory : WeaponAbstractFactory
    {
        protected delegate IWeapon CreateInstanceDelegate(Point point);
        
        List<CreateInstanceDelegate> methodsForCasual = new List<CreateInstanceDelegate>();
        List<CreateInstanceDelegate> methodsForFrequent = new List<CreateInstanceDelegate>();
        List<CreateInstanceDelegate> methodsForRare = new List<CreateInstanceDelegate>();
        List<CreateInstanceDelegate> methodsForLegendary = new List<CreateInstanceDelegate>();
        public WeaponFactory()
        {
            methodsForCasual.AddRange(new CreateInstanceDelegate[] { 
                GetSimplePistol,GetOldSword
            });
            methodsForFrequent.AddRange(new CreateInstanceDelegate[] { 
                GetIronSword,GetRifle
            });
            methodsForRare.AddRange(new CreateInstanceDelegate[] { 
                GetDamaskSword,GetAutomat
            });
            methodsForLegendary.AddRange(new CreateInstanceDelegate[]{
                GetGutsSword,GetMachinegun
            });
        }
        public override IWeapon CreateStuff(Point point)
        {
            Random random = new Random();
            Thread.Sleep(10);
            int rarNum = random.Next(1, 100);
            RarityOfStuff rarityParam = StuffRandomiser.RoundRarity(rarNum);
            switch (rarityParam) {
                case RarityOfStuff.Casual:
                    Thread.Sleep(10);
                    int methodNum = random.Next(0, methodsForCasual.Count);
                    return methodsForCasual[methodNum].Invoke(point);
                case RarityOfStuff.Frequent:
                    Thread.Sleep(10);
                    methodNum = random.Next(0, methodsForFrequent.Count);
                    return methodsForFrequent[methodNum].Invoke(point);
                case RarityOfStuff.Rare:
                    Thread.Sleep(10);
                    methodNum = random.Next(0, methodsForRare.Count);
                    return methodsForRare[methodNum].Invoke(point);
                case RarityOfStuff.Legendary:
                    Thread.Sleep(10);
                    methodNum = random.Next(0,methodsForLegendary.Count);
                    return methodsForLegendary[methodNum].Invoke(point);
                default: throw new Exception();
            }
        }

        public IWeapon GetSimplePistol(Point point)
        {
            List<IBullet> bullets = new List<IBullet>();
            for (int i = 0; i < 30; i++)
                bullets.Add(new SimpleBullet(BulletType.SimplePistolBullet, 10, RarityOfStuff.Casual, point,
                    "Bullets for pistol", 2, 2));
            return new ShootingWeapon(ShootingWeaponType.SimplePistol,BulletType.SimplePistolBullet,RarityOfStuff.Casual,
                point,7,2,"Pistol",3,5,0,500,30,bullets);
        }

        public IWeapon GetOldSword(Point point) {
            return new ColdWeapon(ColdWeaponType.OldSword,RarityOfStuff.Casual,point,6,2,"Old Weapon",
                6,12,10,100);
        }

        public IWeapon GetIronSword(Point place)
        {
            return new ColdWeapon(ColdWeaponType.IronSword,RarityOfStuff.Frequent,place,6,2,"Iron Sword",
                9,13,13,100);
        }

        public IWeapon GetRifle(Point place)
        {
            List<IBullet> bullets = new List<IBullet>();
            for (int i = 0; i < 20; i++)
                bullets.Add(new SimpleBullet(BulletType.RifleBullet, 10, RarityOfStuff.Frequent, place,
                    "Bullets for Rifle", 2, 2));
            return new ShootingWeapon(ShootingWeaponType.Rifle,BulletType.RifleBullet,RarityOfStuff.Frequent,
                place,15,2,"Rifle",10,15,0,1500,20,bullets);
        }

        public IWeapon GetDamaskSword(Point place) {
            return new ColdWeapon(ColdWeaponType.DamaskSword,RarityOfStuff.Rare,place,5,2,"Damask Sword",
                13,21,15,80);
        }

        public IWeapon GetAutomat(Point place) {
            List<IBullet> bullets = new List<IBullet>();
            for (int i = 0; i < 60; i++)
                bullets.Add(new SimpleBullet(BulletType.AutomatBullet, 10, RarityOfStuff.Rare, place,
                    "Bullets for Automat", 2, 2));
            return new ShootingWeapon(ShootingWeaponType.Automat,BulletType.AutomatBullet,RarityOfStuff.Rare,
                place,12,2,"Simple Automat",8,12,0,100,60,bullets);
        }

        public IWeapon GetGutsSword(Point place) {
            return new ColdWeapon(ColdWeaponType.GutsSword,RarityOfStuff.Legendary,place,30,5,"Sword of Berserk",
                20,30,40,2000);
        }

        public IWeapon GetMachinegun(Point place) {
            List<IBullet> bullets = new List<IBullet>();
            for (int i = 0; i < 100; i++)
                bullets.Add(new SimpleBullet(BulletType.MachinegunBullet, 10, RarityOfStuff.Legendary, place,
                    "Bullets for machinegun", 2, 2));
            return new ShootingWeapon(ShootingWeaponType.Machinegun,BulletType.MachinegunBullet,
                RarityOfStuff.Legendary,place,15,3,"Machinegun",6,10,0,10,100,bullets);
        }
    }
}

using GameClasses.GameObjects;
using GameClasses.GameObjects.Poisons;
using GameClasses.Weapons.ShootingWeapons.Bullets;
using System.Drawing;
using System.Collections.Generic;
using System;
using System.Threading;
using GameClasses.Enums;
using GameClasses.Weapons.Bullets;

namespace GameClasses.Factory.StuffFactory
{
    public class ChestStuffFactory : AbstractGameElementFactory
    {
        protected delegate IGenerable CreateInstanceDelegate(Point place);

        List<CreateInstanceDelegate> methodsForCasual = new List<CreateInstanceDelegate>();
        List<CreateInstanceDelegate> methodsForFrequent = new List<CreateInstanceDelegate>();
        List<CreateInstanceDelegate> methodsForRare = new List<CreateInstanceDelegate>();
        List<CreateInstanceDelegate> methodsForLegendary = new List<CreateInstanceDelegate>();

        public ChestStuffFactory() {
            methodsForCasual.AddRange(new CreateInstanceDelegate[] { GetPistolBullets, GetWeakPoison,GetWeapon });
            methodsForFrequent.AddRange(new CreateInstanceDelegate[] { GetRifleBullets,GetWeapon,GetSimplePoison});
            methodsForRare.AddRange(new CreateInstanceDelegate[] { GetAutomatBullets, GetWeapon, GetMediumPoison });
            methodsForLegendary.AddRange(new CreateInstanceDelegate[] { GetMachinegunBullets, GetWeapon,GetLegendaryPoison });
        }

        public override IGenerable GetGameObject(Point place)
        {
            List<IGenerable> totalList = new List<IGenerable>();
            Random random = new Random();
            Thread.Sleep(10);
            int rarityParam = random.Next(0,100);
            RarityOfStuff rarity = StuffRandomiser.RoundRarity(rarityParam);
            switch (rarity)
            {
                case RarityOfStuff.Casual:
                    Thread.Sleep(10);
                    int methodNum = random.Next(0, methodsForCasual.Count);
                    totalList.Add(methodsForCasual[methodNum].Invoke(place));
                    break;
                case RarityOfStuff.Frequent:
                    Thread.Sleep(10);
                    methodNum = random.Next(0, methodsForFrequent.Count);
                    totalList.Add(methodsForFrequent[methodNum].Invoke(place));
                    break;
                case RarityOfStuff.Rare:
                    Thread.Sleep(10);
                    methodNum = random.Next(0, methodsForRare.Count);
                    return methodsForRare[methodNum].Invoke(place);
                case RarityOfStuff.Legendary:
                    Thread.Sleep(10);
                    methodNum = random.Next(0, methodsForLegendary.Count);
                    return methodsForLegendary[methodNum].Invoke(place);
                default: throw new Exception();
            }
            Thread.Sleep(10);
            int randomNumber = random.Next(0,totalList.Count);
            return totalList[randomNumber];
        }

        public IGenerable GetPistolBullets(Point place)
        {
            List<IBullet> bullets = new List<IBullet>();
            for (int i = 0; i < 30; i++)
                bullets.Add(new SimpleBullet(BulletType.SimplePistolBullet,10,RarityOfStuff.Casual,place,
                    "Bullets for pistol",2,2));
            return new BulletClip(bullets,BulletType.SimplePistolBullet,place,4,4);
        }

        public IGenerable GetRifleBullets(Point place) {
            List<IBullet> bullets = new List<IBullet>();
            for (int i = 0; i < 20; i++)
                bullets.Add(new SimpleBullet(BulletType.RifleBullet,10,RarityOfStuff.Frequent,place,
                    "Bullets for Rifle",2,2));
            return new BulletClip(bullets,BulletType.RifleBullet,place,4,4);
        }

        public IGenerable GetAutomatBullets(Point place) {
            List<IBullet> bullets = new List<IBullet>();
            for (int i = 0; i < 60; i++)
                bullets.Add(new SimpleBullet(BulletType.AutomatBullet,10,RarityOfStuff.Rare,place,
                    "Bullets for Automat",2,2));
            return new BulletClip(bullets,BulletType.AutomatBullet,place,4,4);
        }

        public IGenerable GetMachinegunBullets(Point place) {
            List<IBullet> bullets = new List<IBullet>();
            for (int i = 0; i < 100; i++)
                bullets.Add(new SimpleBullet(BulletType.MachinegunBullet,10,RarityOfStuff.Legendary,place,
                    "Bullets for machinegun",2,2));
            return new BulletClip(bullets,BulletType.MachinegunBullet,place,4,4);
        }

        protected IGenerable GetWeakPoison(Point place)
        {
            return new HealthPoison(PoisonType.MiniatureHealthPoison,RarityOfStuff.Casual,place,
                "Miniature health poison",2,4,5);
        }

        protected IGenerable GetSimplePoison(Point place)
        {
            return new HealthPoison(PoisonType.SmallHealthPoison,RarityOfStuff.Frequent,place,
                "Small health poison",2,6,10);
        }

        protected IGenerable GetMediumPoison(Point place)
        {
            return new HealthPoison(PoisonType.MediumHealthPoison,RarityOfStuff.Rare,place,
                "Medium health poison",4,8,15);
        }

        protected IGenerable GetLegendaryPoison(Point place)
        {
            return new HealthPoison(PoisonType.BigHealthPoison,RarityOfStuff.Legendary,place,
                "Big health poison",6,10,20);
        }

        protected IGenerable GetWeapon(Point place) {
            return new WeaponInChest(new WeaponFactory().CreateStuff(place));
        }
    }
}

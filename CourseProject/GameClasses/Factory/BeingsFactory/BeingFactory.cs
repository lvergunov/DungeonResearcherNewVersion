using GameClasses.Beings;
using GameClasses.GameMap;
using GameClasses.Weapons;
using GameClasses.Enums;
using System;
using System.Drawing;
using System.Threading;
using System.Collections.Generic;

namespace GameClasses.Factory.BeingsFactory
{
    public class BeingFactory : BeingAbstractFactory
    {
        protected delegate Enemy CreateInstanceDelegate(Room roomToSpawn, IWeapon weapon, Point point);

        protected List<CreateInstanceDelegate> methodsForCasual = new List<CreateInstanceDelegate>();
        protected List<CreateInstanceDelegate> methodsForFrequent = new List<CreateInstanceDelegate>();
        protected List<CreateInstanceDelegate> methodsForRare = new List<CreateInstanceDelegate>();
        protected List<CreateInstanceDelegate> methodsForLegendary = new List<CreateInstanceDelegate>();

        public BeingFactory()
        {
            methodsForCasual.Add(GetWeakEnemy);
            methodsForFrequent.Add(GetSimpleEnemy);
            methodsForRare.Add(GetStrongEnemy);
            methodsForLegendary.Add(GetLegendaryEnemy);
        }

        public override Enemy Create(Room roomToSpawn, IWeapon weapon, Point point)
        {
            Random random = new Random();
            Thread.Sleep(10);
            RarityOfStuff rarity = StuffRandomiser.RoundRarity(random.Next(1,100));
            switch (rarity) {
                case RarityOfStuff.Casual:
                    Thread.Sleep(10);
                    return methodsForCasual[random.Next(0, methodsForCasual.Count)].Invoke(roomToSpawn,weapon,point);
                case RarityOfStuff.Frequent:
                    Thread.Sleep(10);
                    return methodsForFrequent[random.Next(0, methodsForFrequent.Count)].Invoke(roomToSpawn, weapon, point);
                case RarityOfStuff.Rare:
                    Thread.Sleep(10);
                    return methodsForRare[random.Next(0, methodsForRare.Count)].Invoke(roomToSpawn, weapon, point);
                case RarityOfStuff.Legendary:
                    Thread.Sleep(10);
                    return methodsForLegendary[random.Next(0, methodsForLegendary.Count)].Invoke(roomToSpawn, weapon, point);
                default: throw new Exception();
            }
        }

        protected Enemy GetWeakEnemy(Room roomToSpawn, IWeapon weapon, Point point)
        {
            return new Enemy(EnemyType.WeakEnemy,roomToSpawn,weapon,3,5,point,8,10,"Skeleton",2,RarityOfStuff.Casual,10);
        }

        protected Enemy GetSimpleEnemy(Room roomToSpawn, IWeapon weapon, Point point)
        {
            return new Enemy(EnemyType.SimpleEnemy,roomToSpawn,weapon,4,9,point,10,10,"Undead",2,RarityOfStuff.Frequent,15);
        }

        protected Enemy GetStrongEnemy(Room roomToSpawn, IWeapon weapon, Point point)
        {
            return new Enemy(EnemyType.StrongEnemy,roomToSpawn,weapon,10,15,point,10,15,"Dungeon Guardian",2,RarityOfStuff.Rare,20);
        }

        protected Enemy GetLegendaryEnemy(Room roomToSpawn, IWeapon weapon, Point point) {
            return new Enemy(EnemyType.LegendaryEnemy,roomToSpawn,weapon,15,20,point,15,18,"Lich King",2,RarityOfStuff.Legendary,30);
        }
    }
}

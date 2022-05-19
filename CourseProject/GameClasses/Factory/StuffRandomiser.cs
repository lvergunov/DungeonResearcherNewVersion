using System;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using GameClasses.Game;
using GameClasses.GameObjects;
using GameClasses.Enums;

namespace GameClasses.Factory
{
    public class StuffRandomiser
    {
        internal static RarityOfStuff RoundRarity(int rarity)
        {
            if (rarity > 0 && rarity <= 50) return RarityOfStuff.Casual;
            if (rarity > 50 && rarity <= 80) return RarityOfStuff.Frequent;
            if (rarity > 80 && rarity <= 95) return RarityOfStuff.Rare;
            if (rarity > 95 && rarity <= 100) return RarityOfStuff.Legendary;
            throw new Exception();
        }
    }
}

using GameClasses.Enums;
using GameClasses.Game;

namespace GameClasses.GameObjects
{
    public interface IGenerable : IObjectOnGameMap,IMovable
    {
       RarityOfStuff Rarity { get; }
    }
}

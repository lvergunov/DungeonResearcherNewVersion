using System.Drawing;
using GameClasses.Enums;

namespace GameClasses.Game
{
    public interface IObjectOnGameMap : IGameComponent
    {
        Point Place { get; }
        Rectangle Colider { get; }
        bool IsVisible { get; }
        bool IsSolid { get; }
        void Show();
        void Hide();
        int FindEuclideanDistance(IObjectOnGameMap onGameMap);
    }
}

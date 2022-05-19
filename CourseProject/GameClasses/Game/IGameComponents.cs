using System.Drawing;

namespace GameClasses.Game
{
    public interface IGameComponent
    {
        string Name { get; }
        int ColliderWidth { get; }
        int ColliderHeight { get; }
    }
}

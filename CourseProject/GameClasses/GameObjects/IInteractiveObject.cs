using System.Drawing;
using GameClasses.Beings;
using GameClasses.Game;

namespace GameClasses.GameObjects
{
    public interface IInteractiveObject
    {
        int RadiusOfInteraction { get; }

        Rectangle ColiderOfInteraction { get; }

        void Use(Hero hero);

        string Description { get; }
    }
}

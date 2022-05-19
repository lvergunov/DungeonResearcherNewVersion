using GameClasses.Beings;

namespace GameClasses.GameObjects.LetsAndTraps
{
    public interface ITrap
    {
        float Damage { get; }
        void PunchHero(Hero hero);
        bool IsReloading { get; }

        bool IsRedrawing { get; }

        void StartCycle();

        void StopCycle();
    }
}

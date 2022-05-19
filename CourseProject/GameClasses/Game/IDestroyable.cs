using GameClasses.Enums;
using GameClasses.Beings;

namespace GameClasses.Game
{
    public interface IDestroyable
    {
        float FullHealth { get; }
        void GetPunched(float damageRate,Being sender); 
        bool IsAlive { get; }
    }
}

using System.Drawing;
using System.Linq;
using System;
using GameClasses.Game;
using GameClasses.Beings;

namespace GameClasses.GameObjects.LetsAndTraps
{
    public class BlockOfWall : BaseBlock, IDestroyable
    {
        public bool IsAlive { get {
                return FullHealth > 0;
            } }
        public float FullHealth { get; protected set; }

        public void GetPunched(float damageRate,Being sender) {
            FullHealth -= damageRate;
        }
        public BlockOfWall(Point location,int side) : base(location, side)
        {
            IsSolid = true;
            IsVisible = true;
            FullHealth = 1.0f;
        }
    }
}

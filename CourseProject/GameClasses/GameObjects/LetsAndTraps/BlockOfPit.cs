using GameClasses.Beings;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace GameClasses.GameObjects.LetsAndTraps
{
    public class BlockOfPit : BaseBlock, ITrap
    {
        public BlockOfPit(Point point, int side) : base(point, side)
        {
            Damage = float.MaxValue;
            IsSolid = false;
        }

        public bool IsReloading { get; } = false;

        public float Damage { get; }

        public async void PunchHero(Hero hero)
        {
            await Task.Run(() =>
            {
                hero.ChangePosition(Place);
                Thread.Sleep(100);
                hero.Hide();
                hero.GetPunched(Damage);
            });
        }

        public bool IsRedrawing { get; private set; }

        public void StartCycle()
        {
            IsRedrawing = true;
            while (IsRedrawing)
            {
                Show();
                Thread.Sleep(2000);
                Hide();
                Thread.Sleep(2000);
            }
        }

        public void StopCycle()
        {
            IsRedrawing = false;
        }
    }
}

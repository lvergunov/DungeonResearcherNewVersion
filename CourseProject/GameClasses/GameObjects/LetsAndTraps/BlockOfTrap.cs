using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using GameClasses.Beings;
using GameClasses.Enums;

namespace GameClasses.GameObjects.LetsAndTraps
{
    public class BlockOfTrap : BaseBlock, ITrap
    {
        public float Damage { get; protected set; }
        public float MaximalDamage { get; }
        public TrapType TypeOfTrap { get; }
        public BlockOfTrap(TrapType trapType, Point location, int side, int reloadTime, float damage)
            : base(location, side)
        {
            ReloadTime = reloadTime;
            MaximalDamage = damage;
            TypeOfTrap = trapType;
            IsSolid = false;
            IsReloading = false;
            Hide();
        }
        public int ReloadTime { get; }

        public bool IsReloading { get; protected set; }

        public async void PunchHero(Hero hero)
        {
            await Task.Run(() =>
            {
                Damage = 0;
                if (!IsReloading)
                {
                    Random random = new Random();
                    Thread thread = new Thread(() => Reload());
                    thread.Start();
                }
            });
        }

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
        public bool IsRedrawing { get; private set; } = false;

        protected void Reload()
        {
            Random random = new Random();
            Thread.Sleep(10);
            Damage = (float)(random.NextDouble() * (MaximalDamage - MaximalDamage / 2) + MaximalDamage / 2);
            Thread.Sleep(10);
            Show();
            IsReloading = true;
            Thread.Sleep(ReloadTime);
            IsReloading = false;
            Hide();
        }
    }
}

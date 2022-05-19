using GameClasses.Enums;
using GameClasses.Game;
using GameClasses.GameObjects;

namespace GameClasses.Weapons.ShootingWeapons.Bullets
{
    public interface IBullet : IGenerable
    {
        BulletType TypeOfBullet { get; }
        float SpeedPerMS { get; }
    }
}

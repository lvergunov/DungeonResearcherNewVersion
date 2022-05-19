namespace GameClasses.Enums
{
    public enum Directions
    {
        Left, Right, Up, Down
    }
    public enum Orientation
    {
        Horizontal, Vertical, Diagonal
    }
    public enum LetType
    {
        Wall, Pit, Trap
    }
    public enum MapType
    {
        TotalCoridor, Star, Chain
    }
    public enum BeingType
    {
        Enemy,Hero
    }
    public enum RarityOfStuff
    {
        Casual = 50, Frequent = 80, Rare = 95, Legendary = 100
    }

    public enum BulletType { 
        SimplePistolBullet,RifleBullet,AutomatBullet,MachinegunBullet
    }

    public enum PoisonType { 
        MiniatureHealthPoison,SmallHealthPoison,MediumHealthPoison,BigHealthPoison
    }

    public enum ShootingWeaponType
    {
        SimplePistol,Rifle,Automat,Machinegun
    }

    public enum ColdWeaponType { 
        OldSword,IronSword,DamaskSword,GutsSword
    }

    public enum EnemyType { 
        WeakEnemy, SimpleEnemy, StrongEnemy, LegendaryEnemy
    }

    public enum TrapType
    {
        SimpleTrap
    }
}

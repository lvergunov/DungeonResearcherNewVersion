using System;

namespace GameClasses.Beings.Decorators
{
    public class HeroDecorator : Hero
    {
        public Hero BeingInstance { get; }
        public HeroDecorator(Hero instance) : base(instance.NativeRoom,instance.MinimalDamage,
            instance.MaximalDamage,instance.Place,instance.AbsoluteWidth,instance.AbsoluteHeight,
            instance.Name,instance.WeaponInHands,instance.SpeedPerMS,instance.FullHealth)
        {
            BeingInstance = instance;
            Arsenal = instance.Arsenal;
            Health = instance.Health;
        }
        public string ParamName { get; protected set; } = "";
    }
}

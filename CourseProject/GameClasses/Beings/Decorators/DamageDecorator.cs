namespace GameClasses.Beings.Decorators
{
    public class DamageDecorator : HeroDecorator
    {
        public float PersentOfIncrease { get; }
        public DamageDecorator(Hero instance, float persent) : base(instance)
        {
            PersentOfIncrease = persent;
            ParamName = "Damage";
        }
        public override float MinimalDamage
        {
            get
            {
                return BeingInstance.MinimalDamage * (1 + PersentOfIncrease / 100);
            }
        }
        public override float MaximalDamage
        {
            get
            {
                return BeingInstance.MaximalDamage * (1 + PersentOfIncrease / 100);
            }
        }
    }
}

namespace GameClasses.Beings.Decorators
{
    public class SpeedDecorator : HeroDecorator
    {
        public float PersentOfIncrease { get; }
        public SpeedDecorator(Hero instance, float persent) : base(instance)
        {
            PersentOfIncrease = persent;
            ParamName = "Speed";
        }
        public override float SpeedPerMS
        {
            get
            {
                return BeingInstance.SpeedPerMS * (1 + PersentOfIncrease / 100);
            }
        }
    }
}

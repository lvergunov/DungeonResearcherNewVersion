namespace GameClasses.Beings.Decorators
{
    public class HealthIncreaseDecorator : HeroDecorator
    {
        public float PersentOfIncrease { get; }
        public HealthIncreaseDecorator(Hero instance,float persent) : base(instance) { 
            PersentOfIncrease = persent;
            ParamName = "Health";
        }
        public override float FullHealth { 
            get 
            {
                return BeingInstance.FullHealth*(1 + PersentOfIncrease / 100);
            } 
        }
    }
}

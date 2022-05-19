using GameClasses.Beings;
using GameClasses.Beings.Decorators;
using System;
using System.Threading;
using System.Collections.Generic;

namespace GameClasses.Factory.DecoratorFactory
{
    public class DecoratorsFactory : AbstractFactoryDecorator
    {
        protected delegate HeroDecorator DecoratorCreationDelegate(Hero being, float persents);
        List<DecoratorCreationDelegate> methods = new List<DecoratorCreationDelegate>();
        public DecoratorsFactory()
        {
            methods.Add(GetHealthDecorator);
            methods.Add(GetDamageDecorator);
            methods.Add(GetSpeedDecorator);
        }
        public override HeroDecorator CreateDecorator(Hero being, float persents)
        {
            Random random = new Random();
            Thread.Sleep(10);
            return methods[random.Next(0, methods.Count)].Invoke(being,persents);
        }
        protected HeroDecorator GetHealthDecorator(Hero being, float persents)
        {
            return new HealthIncreaseDecorator(being,persents);
        }

        protected HeroDecorator GetDamageDecorator(Hero being, float persents) {
            return new DamageDecorator(being,persents);
        }

        protected HeroDecorator GetSpeedDecorator(Hero being, float persents)
        {
            return new SpeedDecorator(being,persents);
        }
    }
}

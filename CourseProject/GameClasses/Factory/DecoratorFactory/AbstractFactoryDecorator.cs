using GameClasses.Beings;
using GameClasses.Beings.Decorators;

namespace GameClasses.Factory.DecoratorFactory
{
    public abstract class AbstractFactoryDecorator
    {
        public abstract HeroDecorator CreateDecorator(Hero being,float persents);
    }
}

using Nancy.Bootstrappers.StructureMap;
using StructureMap;

namespace fakenewsisor.server
{
    public class Bootstrapper : StructureMapNancyBootstrapper
    {
        private readonly StructureMapContainerBuilder _containerBuilder;

        public Bootstrapper(StructureMapContainerBuilder builder)
        {
            this._containerBuilder = builder;
        }

        protected override IContainer GetApplicationContainer()
        {
            return _containerBuilder.Build();
        }
    }
}
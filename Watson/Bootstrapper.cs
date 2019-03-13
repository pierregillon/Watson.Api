using Watson.Infrastructure;
using Nancy.Bootstrappers.StructureMap;
using StructureMap;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Watson.Server
{
    public class Bootstrapper : StructureMapNancyBootstrapper
    {
        private AppSettings settings;

        public Bootstrapper(AppSettings settings)
        {
            this.settings = settings;
        }

        protected override IContainer GetApplicationContainer()
        {
            var containerBuilder = new StructureMapContainerBuilder();
            return containerBuilder.Build(settings);
        }
    }
}
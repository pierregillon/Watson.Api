using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRSlite.Events;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Nancy.Owin;
using StructureMap;

namespace Watson.Server
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseOwin(x => x.UseNancy());
        }

        public async void Initialize(IContainer container)
        {
            var eventStore = container.GetInstance<Watson.Infrastructure.EventStore>();
            var eventPublisher = container.GetInstance<IEventPublisher>();
            var events = await eventStore.GetAllEventsFromBegining();
            foreach (var @event in events) {
                await eventPublisher.Publish(@event);
            }
        }
    }
}

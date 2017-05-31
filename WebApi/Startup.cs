using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MessagingService.Contracts;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;

namespace WebApi
{
    public class Startup
    {
        //This is called by the runtime and is where you bootstrap your services
        public void ConfigureServices(IServiceCollection services)
        {
            //ignore nulls for from body mvc deserialization
            services.AddMvc().AddJsonOptions(jsonOptions =>
            {
                jsonOptions.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            }); 

            //we will be using the msft DI container to inject our messaging service. 
            //Here, we construct the interface by creating a service proxy to the fabric messaging service
            //we only introduce a dependency on the interface contract itself, and specify the implementation
            //simply with the name of the service. Holy decoupling batman!
            services.AddTransient<IMessagingService>(ctx =>
            {
                return ServiceProxy.Create<IMessagingService>(
                    new Uri("fabric:/HelloWorldLevelTwo/MessagingService")
                );
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            app.UseStaticFiles();
            app.UseDefaultFiles();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMvc();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ConsoleApp1.AppManagement
{
    public class AppManager
    {
        private readonly ILogger<AppManager> Logger;

        public AppManager(ILogger<AppManager> logger)
        {
            Logger = logger;
        }

        private ICollection<AppEntry> Apps = new List<AppEntry>();

        public RequestDelegate Middleware(RequestDelegate next)
        {
            return context => {
                foreach (var appEntry in Apps)
                {
                    if (context.Request.Path.StartsWithSegments("/"+appEntry.Startup.Name))
                    {
                        return appEntry.RequestDelegate(context);
                    }
                }

                return next(context);
            };
        }

        public void StartApp(IAppStartup app, IApplicationBuilder applicationBuilder)
        {
            Logger.LogInformation($"Starting app {app.Name}");
            try
            {
                var serviceCollection = new ServiceCollection();
                app.ConfigureServices(serviceCollection);
                var newAppBuilder = applicationBuilder.New();
                newAppBuilder.ApplicationServices = serviceCollection.BuildServiceProvider();
                app.Configure(newAppBuilder);
                Apps.Add(new AppEntry()
                {
                    Startup = app,
                    RequestDelegate = newAppBuilder.Build()
                });
                Logger.LogInformation($"Started app {app.Name}");
            }
            catch (Exception e)
            {
                Logger.LogError(e, $"Could not start app {app.Name}");
            }
        }

        public void StopApp(string name)
        {
            Logger.LogInformation($"Stopping app {name}");
            try
            {
                var appEntry = Apps.First(entry => entry.Startup.Name == name);
                Apps.Remove(appEntry);
                Logger.LogInformation($"Stopped app {name}");
            }
            catch (Exception e)
            {
                Logger.LogError(e, $"Could not start app {name}");
            }
        }

        private class AppEntry
        {
            public IAppStartup Startup { get; set; }
            public RequestDelegate RequestDelegate { get; set; }
        }
    }
}
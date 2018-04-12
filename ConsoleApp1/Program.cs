using ConsoleApp1.AppManagement;
using ConsoleApp1.Words;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<MainStartup>()
                .Build()
                .Run();
        }
    }

    public class MainStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(builder => {
                builder.AddConsole();
                builder.SetMinimumLevel(LogLevel.Information);
            })
                .AddTransient<WordWritingHandler>()
                .AddAppManager()
                .AddTransient<IWordProvider, MainWordProvider>();
        }

        public void Configure(IApplicationBuilder applicationBuilder)
        {
            applicationBuilder
                .Map("/main", app => app.Run(context => context.Response.WriteAsync("Main")));
            
            ConfigureAppManagerHandlers(applicationBuilder);
            applicationBuilder.UseAppManager();
        }

        private static void ConfigureAppManagerHandlers(IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.Map("/start/1",
                app => app.Run(context => {
                    context.RequestServices.GetRequiredService<AppManager>()
                        .StartApp(new App1Startup(), applicationBuilder);
                    return context.Response.WriteAsync("started 1");
                }));
            applicationBuilder.Map("/start/2",
                app => app.Run(context => {
                    context.RequestServices.GetRequiredService<AppManager>()
                        .StartApp(new App2Startup(), applicationBuilder);
                    return context.Response.WriteAsync("started 2");
                }));

            applicationBuilder.Map("/stop/1",
                app => app.Run(context => {
                    context.RequestServices.GetRequiredService<AppManager>()
                        .StopApp("App1");
                    return context.Response.WriteAsync("stopped 1");
                }));
            applicationBuilder.Map("/stop/2",
                app => app.Run(context => {
                    context.RequestServices.GetRequiredService<AppManager>()
                        .StopApp("App2");
                    return context.Response.WriteAsync("stopped 2");
                }));
        }
    }

    public class App1Startup : IAppStartup
    {
        public string Name => "App1";

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddTransient<WordWritingHandler>()
                .AddTransient<IWordProvider, App1WordProvider>();
        }

        public void Configure(IApplicationBuilder applicationBuilder)
        {
            applicationBuilder
                .Map("/main", app => app.Run(context => context.Response.WriteAsync("App1")))
                .Map("/App1",
                    app => {
                        var handler = app.ApplicationServices.GetRequiredService<WordWritingHandler>();
                        app.Run(handler.Handle);
                    });
        }
    }

    public class App2Startup : IAppStartup
    {
        public string Name => "App2";

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddTransient<WordWritingHandler>()
                .AddTransient<IWordProvider, App2WordProvider>();
        }

        public void Configure(IApplicationBuilder applicationBuilder)
        {
            applicationBuilder
                .Map("/main", app => app.Run(context => context.Response.WriteAsync("App2")));
            applicationBuilder
                .Map("/App2",
                    app => {
                        var handler = app.ApplicationServices.GetRequiredService<WordWritingHandler>();
                        app.Run(handler.Handle);
                    });
        }
    }


}

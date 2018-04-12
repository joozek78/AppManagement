using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApp1.AppManagement
{
    public static class AppManagerExtensions
    {
        public static IApplicationBuilder UseAppManager(this IApplicationBuilder applicationBuilder) =>
            applicationBuilder.Use(applicationBuilder.ApplicationServices.GetRequiredService<AppManager>().Middleware);

        public static IServiceCollection AddAppManager(this IServiceCollection serviceCollection) =>
            serviceCollection.AddSingleton<AppManager>();
    }
}
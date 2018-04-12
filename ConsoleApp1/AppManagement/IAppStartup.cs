using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApp1.AppManagement
{
    public interface IAppStartup
    {
        void ConfigureServices(IServiceCollection services);
        void Configure(IApplicationBuilder applicationBuilder);

        string Name { get; }
    }
}
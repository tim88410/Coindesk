using CurrencyLink.Infrastructure.Service;
using Microsoft.Extensions.DependencyInjection;

namespace CurrencyLink.Infrastructure
{
    public static class DependencyInjectionSetup
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // 註冊 CoindeskService
            //services.AddScoped<ICoindeskService, CoindeskService>();

            return services;
        }
    }
}

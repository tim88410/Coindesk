using CurrencyLink.Domain.Service;
using CurrencyLink.Infrastructure.Service;
using Microsoft.AspNetCore.Authorization;

namespace CurrencyLink
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

using CurrencyLink.Domain.Service;
using CurrencyLink.Infrastructure.Service;
using Microsoft.AspNetCore.Authorization;

namespace CurrencyLink
{
    public static class DependencyInjectionSetup
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {

            return services;
        }
    }
}

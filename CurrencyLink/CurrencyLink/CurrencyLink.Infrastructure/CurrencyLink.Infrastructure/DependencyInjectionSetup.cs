using CurrencyLink.Domain.Repositories.CoindeskAPIClient;
using CurrencyLink.Domain.Service;
using CurrencyLink.Infrastructure.Repositories.Coindesk;
using CurrencyLink.Infrastructure.Repositories.CoindeskAPIClient;
using CurrencyLink.Infrastructure.Service;
using Microsoft.Extensions.DependencyInjection;
using RestSharp;

namespace CurrencyLink.Infrastructure
{
    public static class DependencyInjectionSetup
    {
        public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services)
        {


            services.AddScoped<ICoindeskAPIClientService, CoindeskAPIClientService>();
            services.AddScoped(sp =>
            {
                var baseUrl = "https://api.coindesk.com/v1/bpi/currentprice.json";
                var options = new RestClientOptions(baseUrl);
                return new RestClient(options);
            });
            services.AddScoped<IRestClientWrapperService, RestClientWrapperService>();
            services.AddScoped<ICoindeskAPIClientRepository, CoindeskAPIClientRepository>();
            services.AddScoped<ICoindeskQueryRepository, CoindeskQueryRepository>();
            services.AddScoped<ICoindeskCommandRepository, CoindeskCommandRepository>();
            return services;
        }
    }
}

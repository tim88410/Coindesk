using CurrencyLink.Domain.Service;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyLink.Infrastructure.Service
{
    public class RestClientWrapperService : IRestClientWrapperService
    {
        private readonly RestClient _client;

        public RestClientWrapperService(RestClient client)
        {
            _client = client;
        }

        public async Task<RestResponse> ExecuteAsync(RestRequest request)
        {
            return await _client.ExecuteAsync(request);
        }
    }
}

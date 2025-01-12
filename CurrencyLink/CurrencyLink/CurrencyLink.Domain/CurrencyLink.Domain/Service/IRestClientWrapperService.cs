using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyLink.Domain.Service
{
    public interface IRestClientWrapperService
    {
        Task<RestResponse> ExecuteAsync(RestRequest request);
    }
}

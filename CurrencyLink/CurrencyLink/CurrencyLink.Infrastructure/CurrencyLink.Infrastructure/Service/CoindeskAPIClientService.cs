using CurrencyLink.Domain.Models;
using CurrencyLink.Domain.Service;
using DBUtility;
using RestSharp;
using System.Text.Json;
using Serilog;

namespace CurrencyLink.Infrastructure.Service
{
    public class CoindeskAPIClientService : ICoindeskAPIClientService
    {
        private readonly IRestClientWrapperService _client;

        public CoindeskAPIClientService(IRestClientWrapperService client)
        {
            _client = client;
        }

        public (int nRet, CoindeskClientDTO coindeskDTO) GetCoindesk()
        {
            var client = new RestClient("https://api.coindesk.com/v1/bpi/currentprice.json");

            var request = new RestRequest();
            int nRet = ErrorCode.KErrNone;
            string apiResponse = string.Empty;

            try
            {
                var response = _client.ExecuteAsync(request).Result;

                // 記錄請求

                if (response.IsSuccessStatusCode)
                {
                    nRet = ErrorCode.KErrNone;
                    apiResponse = response.Content ?? string.Empty;
                }
                else
                {
                    nRet = ErrorCode.KEventHTTP;
                    apiResponse = $"API 呼叫失敗。狀態碼: {response.StatusCode}, 錯誤訊息: {response.ErrorMessage}";
                }

                var requestBody = $"Request:  {response.ResponseUri} {response.StatusCode} {response.Content}  {response.ErrorMessage}";
                Log.Information(requestBody);
            }
            catch (Exception ex)
            {
                nRet = ErrorCode.KErrIntegrationException;
                apiResponse = $"發生錯誤: {ex.Message}";
            }

            if (nRet == ErrorCode.KErrNone)
            {
                try
                {
                    var coindeskData = JsonSerializer.Deserialize<CoindeskClientDTO.CoindeskClientResponse>(apiResponse);

                    CoindeskClientDTO coindeskDTO = new CoindeskClientDTO()
                    {
                        UpdatedISO = coindeskData.time.updatedISO,
                        CurrencyInfos = coindeskData.bpi.Select(s => new CoindeskClientDTO.CurrencyInfo
                        {
                            currencyCode = s.Key,
                            code = s.Value.code,
                            symbol = s.Value.symbol,
                            rate = s.Value.rate,
                            description = s.Value.description,
                            rate_float = s.Value.rate_float
                        }).ToList()
                    };

                    return (ErrorCode.KErrNone, coindeskDTO);
                }
                catch (JsonException jsonEx)
                {
                    Console.WriteLine($"JSON 解析錯誤: {jsonEx.Message}");
                    return (ErrorCode.KErrJsonParseError, new CoindeskClientDTO());
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"未知錯誤: {ex.Message}");
                    return (ErrorCode.KErrErrorStatus, new CoindeskClientDTO());
                }
            }

            return (nRet, new CoindeskClientDTO());
        }
    }
}
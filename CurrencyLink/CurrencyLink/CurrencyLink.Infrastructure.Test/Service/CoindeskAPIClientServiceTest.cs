using CurrencyLink.Domain.Models;
using CurrencyLink.Domain.Service;
using CurrencyLink.Infrastructure.Service;
using DBUtility;
using Moq;
using RestSharp;
using System.Linq;
using System.Net;
using Xunit;

namespace CurrencyLink.Infrastructure.Test.Service
{
    public class CoindeskAPIClientServiceTest
    {
        [Fact]
        public void GetCoindesk_SuccessfulResponse_ReturnsData()
        {
            // Arrange
            var mockClient = new Mock<IRestClientWrapperService>();
            var fakeResponse = new RestResponse
            {
                IsSuccessStatusCode = true,
                Content = @"{
                    ""time"": {
                        ""updatedISO"": ""2025-01-01T00:00:00Z""
                    },
                    ""bpi"": {
                        ""USD"": {
                            ""code"": ""USD"",
                            ""symbol"": ""$"",
                            ""rate"": ""20,000.00"",
                            ""description"": ""United States Dollar"",
                            ""rate_float"": 20000.00
                        }
                    }
                }"
            };

            mockClient.Setup(c => c.ExecuteAsync(It.IsAny<RestRequest>()))
                      .ReturnsAsync(fakeResponse);

            var service = new CoindeskAPIClientService(mockClient.Object);

            // Act
            var result = service.GetCoindesk();

            // Assert
            Assert.Equal(ErrorCode.KErrNone, result.nRet);
            Assert.NotNull(result.coindeskDTO);
            Assert.Single(result.coindeskDTO.CurrencyInfos);
            Assert.Equal("USD", result.coindeskDTO.CurrencyInfos[0].code);
            Assert.Equal("$", result.coindeskDTO.CurrencyInfos[0].symbol);
            Assert.Equal("20,000.00", result.coindeskDTO.CurrencyInfos[0].rate);
            Assert.Equal("United States Dollar", result.coindeskDTO.CurrencyInfos[0].description);
            Assert.Equal(20000.00m, result.coindeskDTO.CurrencyInfos[0].rate_float);
        }

        [Fact]
        public void GetCoindesk_ApiFailure_ReturnsError()
        {
            // Arrange
            var mockClient = new Mock<IRestClientWrapperService>();
            var fakeResponse = new RestResponse
            {
                IsSuccessStatusCode = false,
                ErrorMessage = "Bad Request"
            };

            mockClient.Setup(c => c.ExecuteAsync(It.IsAny<RestRequest>()))
                      .ReturnsAsync(fakeResponse);

            var service = new CoindeskAPIClientService(mockClient.Object);

            // Act
            var result = service.GetCoindesk();

            // Assert
            Assert.Equal(ErrorCode.KEventHTTP, result.nRet);
            Assert.Empty(result.coindeskDTO.CurrencyInfos);
        }

        [Fact]
        public void GetCoindesk_JsonParseError_ReturnsError()
        {
            // Arrange
            var mockClient = new Mock<IRestClientWrapperService>();
            var fakeResponse = new RestResponse
            {
                IsSuccessStatusCode = true,
                Content = "Invalid JSON"
            };

            mockClient.Setup(c => c.ExecuteAsync(It.IsAny<RestRequest>()))
                      .ReturnsAsync(fakeResponse);

            var service = new CoindeskAPIClientService(mockClient.Object);

            // Act
            var result = service.GetCoindesk();

            // Assert
            Assert.Equal(ErrorCode.KErrJsonParseError, result.nRet);
            Assert.Empty(result.coindeskDTO.CurrencyInfos);
        }
    }
}

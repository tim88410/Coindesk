using CurrencyLink.Application.Commands.CoindeskAPIClient;
using CurrencyLink.Domain.Models;
using CurrencyLink.Domain.Repositories.CoindeskAPIClient;
using CurrencyLink.Domain.Service;
using DBUtility;
using MediatR;
using Moq;

namespace CurrencyLinkTest.Application.Commands.CoindeskAPIClient
{
    public class CoindeskAPIClientHandlerTest
    {
        private class FakeCoindeskAPIClientServiceSuccessful : ICoindeskAPIClientService
        {
            public (int nRet, CoindeskClientDTO coindeskDTO) GetCoindesk()
            {
                return (ErrorCode.KErrNone, new CoindeskClientDTO());
            }
        }

        private class FakeCoindeskAPIClientServiceHTTPResponseFailed : ICoindeskAPIClientService
        {
            public (int nRet, CoindeskClientDTO coindeskDTO) GetCoindesk()
            {
                return (ErrorCode.KEventHTTP, new CoindeskClientDTO());
            }
        }
        private class FakeCoindeskAPIClientServiceIntegrationException : ICoindeskAPIClientService
        {
            public (int nRet, CoindeskClientDTO coindeskDTO) GetCoindesk()
            {
                return (ErrorCode.KErrIntegrationException, new CoindeskClientDTO());
            }
        }
        private class FakeCoindeskAPIClientServiceJsonParseException : ICoindeskAPIClientService
        {
            public (int nRet, CoindeskClientDTO coindeskDTO) GetCoindesk()
            {
                return (ErrorCode.KErrJsonParseError, new CoindeskClientDTO());
            }
        }

        private class FakeCoindeskAPIClientServiceJsonParseUnexpectedException : ICoindeskAPIClientService
        {
            public (int nRet, CoindeskClientDTO coindeskDTO) GetCoindesk()
            {
                return (ErrorCode.KErrErrorStatus, new CoindeskClientDTO());
            }
        }
        private class FakeCoindeskAPIClientRepositorySuccessful : ICoindeskAPIClientRepository
        {
            public Task<int> ApiUpdate(string currencyUpdateParameter)
            {
                return Task.FromResult(ErrorCode.KErrNone);
            }
        }
        private class FakeCoindeskAPIClientRepositoryFailed : ICoindeskAPIClientRepository
        {
            public Task<int> ApiUpdate(string currencyUpdateParameter)
            {
                return Task.FromResult(ErrorCode.KErrDBError);
            }
        }

        [Fact]
        public async Task CoindeskAPIClientServiceSuccessful()
        {
            var mediatorMock = new Mock<IMediator>();
            CoindeskAPIClientHandler coindeskAPIClientHandler = new CoindeskAPIClientHandler(new FakeCoindeskAPIClientServiceSuccessful(), new FakeCoindeskAPIClientRepositorySuccessful());

            var result = await coindeskAPIClientHandler.Handle(new CoindeskAPIClientCommand { }, CancellationToken.None);

            Assert.Equal(ErrorCode.KErrNone, result);
        }
        [Fact]
        public async Task CoindeskAPIClientServiceHTTPResponseFailed()
        {
            var mediatorMock = new Mock<IMediator>();
            CoindeskAPIClientHandler coindeskAPIClientHandler = new CoindeskAPIClientHandler(new FakeCoindeskAPIClientServiceHTTPResponseFailed(), new FakeCoindeskAPIClientRepositoryFailed());

            var result = await coindeskAPIClientHandler.Handle(new CoindeskAPIClientCommand { }, CancellationToken.None);

            Assert.Equal(ErrorCode.KEventHTTP, result);
        }
        [Fact]
        public async Task CoindeskAPIClientServiceIntegrationException()
        {
            var mediatorMock = new Mock<IMediator>();
            CoindeskAPIClientHandler coindeskAPIClientHandler = new CoindeskAPIClientHandler(new FakeCoindeskAPIClientServiceIntegrationException(), new FakeCoindeskAPIClientRepositoryFailed());

            var result = await coindeskAPIClientHandler.Handle(new CoindeskAPIClientCommand { }, CancellationToken.None);

            Assert.Equal(ErrorCode.KErrIntegrationException, result);
        }
        [Fact]
        public async Task CoindeskAPIClientServiceJsonParseException()
        {
            var mediatorMock = new Mock<IMediator>();
            CoindeskAPIClientHandler coindeskAPIClientHandler = new CoindeskAPIClientHandler(new FakeCoindeskAPIClientServiceJsonParseException(), new FakeCoindeskAPIClientRepositoryFailed());

            var result = await coindeskAPIClientHandler.Handle(new CoindeskAPIClientCommand { }, CancellationToken.None);

            Assert.Equal(ErrorCode.KErrJsonParseError, result);
        }
        //JsonParseUnexpectedException

        [Fact]
        public async Task CoindeskAPIClientServiceJsonParseUnexpectedException()
        {
            var mediatorMock = new Mock<IMediator>();
            CoindeskAPIClientHandler coindeskAPIClientHandler = new CoindeskAPIClientHandler(new FakeCoindeskAPIClientServiceJsonParseUnexpectedException(), new FakeCoindeskAPIClientRepositoryFailed());

            var result = await coindeskAPIClientHandler.Handle(new CoindeskAPIClientCommand { }, CancellationToken.None);

            Assert.Equal(ErrorCode.KErrErrorStatus, result);
        }
    }
}

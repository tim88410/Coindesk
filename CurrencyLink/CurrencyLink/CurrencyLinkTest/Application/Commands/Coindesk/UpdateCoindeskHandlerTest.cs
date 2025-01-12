using CurrencyLink.Application.Commands.Coindesk;
using CurrencyLink.Infrastructure.Models.Coindesk;
using CurrencyLink.Infrastructure.Repositories.Coindesk;
using DBUtility;
using MediatR;
using Moq;

namespace CurrencyLinkTest.Application.Commands.Coindesk
{
    public class UpdateCoindeskHandlerTest
    {
        private class FakeCoindeskCommandRepositoryUpdateSuccessful : ICoindeskCommandRepository
        {
            public Task<int> UpdateAsync(CoindeskCommand.CoindeskParameter request)
            {
                return Task.FromResult(ErrorCode.KErrNone);
            }
            //本次測試未用到
            public Task<int> DeleteAsync(CoindeskCommand.CoindeskDeleteParameter request)
            {
                return Task.FromResult(ErrorCode.KErrNone);
            }
        }

        private class FakeCoindeskCommandRepositoryUpdateFailed : ICoindeskCommandRepository
        {
            public Task<int> UpdateAsync(CoindeskCommand.CoindeskParameter request)
            {
                return Task.FromResult(ErrorCode.KErrDBError);
            }
            //本次測試未用到
            public Task<int> DeleteAsync(CoindeskCommand.CoindeskDeleteParameter request)
            {
                return Task.FromResult(ErrorCode.KErrNone);
            }
        }
        [Fact]
        public async Task UpdateCoindeskSuccessful()
        {
            var mediatorMock = new Mock<IMediator>();
            UpdateCoindeskHandler updateCoindeskHandler = new UpdateCoindeskHandler(new FakeCoindeskCommandRepositoryUpdateSuccessful());

            var result = await updateCoindeskHandler.Handle(new UpdateCoindeskCommand
            {
                Id = 1,
                Code = string.Empty,
                CodeName = string.Empty,
                Symbol = string.Empty,
                Description = string.Empty,
                Rate = string.Empty,
                RateFloat = 0,
                CurrencyCode = string.Empty
            }, CancellationToken.None);

            Assert.Equal(ErrorCode.KErrNone, result);
        }

        [Fact]
        public async Task UpdateCoindeskFailed()
        {
            var mediatorMock = new Mock<IMediator>();
            UpdateCoindeskHandler updateCoindeskHandler = new UpdateCoindeskHandler(new FakeCoindeskCommandRepositoryUpdateFailed());

            var result = await updateCoindeskHandler.Handle(new UpdateCoindeskCommand
            {
                Id = 1,
                Code = string.Empty,
                CodeName = string.Empty,
                Symbol = string.Empty,
                Description = string.Empty,
                Rate = string.Empty,
                RateFloat = 0,
                CurrencyCode = string.Empty
            }, CancellationToken.None);

            Assert.Equal(ErrorCode.KErrDBError, result);
        }
    }
}



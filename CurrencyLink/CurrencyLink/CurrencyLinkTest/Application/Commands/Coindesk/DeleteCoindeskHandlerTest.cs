using CurrencyLink.Application.Commands.Coindesk;
using CurrencyLink.Infrastructure.Models.Coindesk;
using CurrencyLink.Infrastructure.Repositories.Coindesk;
using DBUtility;
using MediatR;
using Moq;

namespace CurrencyLinkTest.Application.Commands.Coindesk
{
    public class DeleteCoindeskHandlerTest
    {
        private class FakeCoindeskCommandRepositoryDeleteSuccessful : ICoindeskCommandRepository
        {
            //本次測試未用到
            public Task<int> UpdateAsync(CoindeskCommand.CoindeskParameter request)
            {
                return Task.FromResult(ErrorCode.KErrNone);
            }
            public Task<int> DeleteAsync(CoindeskCommand.CoindeskDeleteParameter request)
            {
                return Task.FromResult(ErrorCode.KErrNone);
            }
        }

        private class FakeCoindeskCommandRepositoryDeleteFailed : ICoindeskCommandRepository
        {
            //本次測試未用到
            public Task<int> UpdateAsync(CoindeskCommand.CoindeskParameter request)
            {
                return Task.FromResult(ErrorCode.KErrNone);
            }
            public Task<int> DeleteAsync(CoindeskCommand.CoindeskDeleteParameter request)
            {
                return Task.FromResult(ErrorCode.KErrDBError);
            }
        }
        [Fact]
        public async Task DeleteCoindeskSuccessful()
        {
            var mediatorMock = new Mock<IMediator>();
            DeleteCoindeskHandler deleteCoindeskHandler = new DeleteCoindeskHandler(new FakeCoindeskCommandRepositoryDeleteSuccessful());

            var result = await deleteCoindeskHandler.Handle(new DeleteCoindeskCommand
            {
                Id = 1
            }, CancellationToken.None);

            Assert.Equal(ErrorCode.KErrNone, result);
        }

        [Fact]
        public async Task DeleteCoindeskFailed()
        {
            var mediatorMock = new Mock<IMediator>();
            DeleteCoindeskHandler deleteCoindeskHandler = new DeleteCoindeskHandler(new FakeCoindeskCommandRepositoryDeleteFailed());

            var result = await deleteCoindeskHandler.Handle(new DeleteCoindeskCommand
            {
                Id = 1
            }, CancellationToken.None);

            Assert.Equal(ErrorCode.KErrDBError, result);
        }
    }
}

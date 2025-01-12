using CurrencyLink.Infrastructure.Models.Coindesk;
using CurrencyLink.Infrastructure.Repositories.Coindesk;
using MediatR;

namespace CurrencyLink.Application.Commands.Coindesk
{
    public class DeleteCoindeskHandler : IRequestHandler<DeleteCoindeskCommand, int>
    {
        private readonly ICoindeskCommandRepository _coindeskCommandRepository;

        public DeleteCoindeskHandler(
            ICoindeskCommandRepository coindeskCommandRepository
            )
        {
            _coindeskCommandRepository = coindeskCommandRepository;
        }

        public async Task<int> Handle(DeleteCoindeskCommand command, CancellationToken cancellationToken)
        {
            var result = await _coindeskCommandRepository.DeleteAsync(new CoindeskCommand.CoindeskDeleteParameter
            {
                Id = command.Id,
            });

            return result;
        }
    }
}

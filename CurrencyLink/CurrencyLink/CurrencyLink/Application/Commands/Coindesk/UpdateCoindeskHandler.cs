using CurrencyLink.Infrastructure.Models.Coindesk;
using CurrencyLink.Infrastructure.Repositories.Coindesk;
using MediatR;

namespace CurrencyLink.Application.Commands.Coindesk
{
    public class UpdateCoindeskHandler : IRequestHandler<UpdateCoindeskCommand, int>
    {
        private readonly ICoindeskCommandRepository _coindeskCommandRepository;

        public UpdateCoindeskHandler(
            ICoindeskCommandRepository coindeskCommandRepository
            )
        {
            _coindeskCommandRepository = coindeskCommandRepository;
        }

        public async Task<int> Handle(UpdateCoindeskCommand command, CancellationToken cancellationToken)
        {
            var result = await _coindeskCommandRepository.UpdateAsync(new CoindeskCommand.CoindeskParameter
            {
                Id = command.Id,
                Code = command.Code,
                CodeName = command.CodeName,
                Symbol = command.Symbol,
                Description = command.Description,
                Rate = command.Rate,
                Rate_float = command.RateFloat,
                CurrencyCode = command.CurrencyCode
            });

            return result;
        }
    }
}

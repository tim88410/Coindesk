using AutoMapper;
using CurrencyLink.Infrastructure.Models.Coindesk;
using CurrencyLink.Infrastructure.Repositories.Coindesk;
using MediatR;

namespace CurrencyLink.Application.Commands.Coindesk
{
    public class UpdateCoindeskHandler : IRequestHandler<UpdateCoindeskCommand, int>
    {
        private readonly ICoindeskCommandRepository _coindeskCommandRepository;
        private readonly IMapper _mapper;

        public UpdateCoindeskHandler(
            ICoindeskCommandRepository coindeskCommandRepository,
            IMapper mapper
            )
        {
            _coindeskCommandRepository = coindeskCommandRepository;
            _mapper = mapper;
        }

        public async Task<int> Handle(UpdateCoindeskCommand command, CancellationToken cancellationToken)
        {
            var coindeskPara = _mapper.Map<CoindeskCommand.CoindeskParameter>(command);
            return await _coindeskCommandRepository.UpdateAsync(coindeskPara);
        }
    }
}

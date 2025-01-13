using AutoMapper;
using CurrencyLink.Infrastructure.Models.Coindesk;
using CurrencyLink.Infrastructure.Repositories.Coindesk;
using MediatR;

namespace CurrencyLink.Application.Queries.Coindesk
{
    public class CoindeskHandler : IRequestHandler<CoindeskRequest, CoindeskResponse?>
    {
        private readonly ICoindeskQueryRepository _coindeskQueryRepository;
        private readonly IMapper _mapper;
        public CoindeskHandler(ICoindeskQueryRepository coindeskQueryRepository,
            IMapper mapper) 
        {
            _coindeskQueryRepository = coindeskQueryRepository;
            _mapper = mapper;
        }

        public async Task<CoindeskResponse?> Handle(CoindeskRequest request, CancellationToken cancellationToken)
        {

            var coindeskPara = _mapper.Map<CoindeskQuery.CoindeskQueryParameter>(request);
            var coindeskQuery = await _coindeskQueryRepository.GetAsync(coindeskPara);
            //DBConnectError
            if (coindeskQuery == null) { 
                return null;
            }

            return new CoindeskResponse {
                CoindeskInfos = _mapper.Map<List<CoindeskResponse.CoindeskInfo>>(coindeskQuery.ToList()),
                Total = coindeskQuery.Select(s => s.TotalItem).FirstOrDefault()
            };
        }
    }
}

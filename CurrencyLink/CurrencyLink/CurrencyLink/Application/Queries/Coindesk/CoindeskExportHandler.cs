using AutoMapper;
using CurrencyLink.Infrastructure.Models.Coindesk;
using CurrencyLink.Infrastructure.Repositories.Coindesk;
using MediatR;

namespace CurrencyLink.Application.Queries.Coindesk
{
    public class CoindeskExportHandler : IRequestHandler<CoindeskExportRequest, CoindeskExportResponse?>
    {
        private readonly ICoindeskQueryRepository _coindeskQueryRepository;
        private readonly IMapper _mapper;
        public CoindeskExportHandler(ICoindeskQueryRepository coindeskQueryRepository,
            IMapper mapper)
        {
            _coindeskQueryRepository = coindeskQueryRepository;
            _mapper = mapper;
        }

        public async Task<CoindeskExportResponse?> Handle(CoindeskExportRequest request, CancellationToken cancellationToken)
        {

            var coindeskPara = _mapper.Map<CoindeskQuery.CoindeskQueryParameter>(request);
            coindeskPara.Page = 1;
            coindeskPara.PageLimit = int.MaxValue;
            var coindeskQuery = await _coindeskQueryRepository.GetAsync(coindeskPara);
            //DBConnectError
            if (coindeskQuery == null)
            {
                return null;
            }

            return new CoindeskExportResponse
            {
                CoindeskInfo = _mapper.Map<List<CoindeskResponse.CoindeskInfo>>(coindeskQuery.ToList())
            };
        }
    }
}

using AutoMapper;
using CurrencyLink.Infrastructure.Repositories.Coindesk;
using MediatR;

namespace CurrencyLink.Application.Queries.Coindesk
{
    public class CoindeskGetOneHandler : IRequestHandler<CoindeskGetOneRequest, IEnumerable<CoindeskResponse.CoindeskInfo>?>
    {
        private readonly ICoindeskQueryRepository _coindeskQueryRepository;
        private readonly IMapper _mapper;
        public CoindeskGetOneHandler(ICoindeskQueryRepository coindeskQueryRepository, IMapper mapper)
        {
            _coindeskQueryRepository = coindeskQueryRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CoindeskResponse.CoindeskInfo>?> Handle(CoindeskGetOneRequest request, CancellationToken cancellationToken)
        {
            var coindeskQuery = await _coindeskQueryRepository.GetOneAsync(request.Id);

            if (coindeskQuery == null)
            {
                return null;
            }
            return _mapper.Map<IEnumerable<CoindeskResponse.CoindeskInfo>>(coindeskQuery.ToList());
        }
    }
}

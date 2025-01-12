using CurrencyLink.Infrastructure.Models.Coindesk;
using CurrencyLink.Infrastructure.Repositories.Coindesk;
using MediatR;

namespace CurrencyLink.Application.Queries.Coindesk
{
    public class CoindeskGetOneHandler : IRequestHandler<CoindeskGetOneRequest, IEnumerable<CoindeskResponse.CoindeskInfo>?>
    {
        private readonly ICoindeskQueryRepository _coindeskQueryRepository;
        public CoindeskGetOneHandler(ICoindeskQueryRepository coindeskQueryRepository)
        {
            _coindeskQueryRepository = coindeskQueryRepository;
        }

        public async Task<IEnumerable<CoindeskResponse.CoindeskInfo>?> Handle(CoindeskGetOneRequest request, CancellationToken cancellationToken)
        {
            var coindeskQuery = await _coindeskQueryRepository.GetOneAsync(request.Id);

            if (coindeskQuery == null)
            {
                return null;
            }
            return coindeskQuery.Select(s => new CoindeskResponse.CoindeskInfo()
            {
                Id = s.Id,
                Code = s.Code,
                CodeName = s.CodeName,
                Symbol = s.Symbol,
                Description = s.Description,
                Rate = s.Rate,
                RateFloat = s.Rate_float,
                CurrencyCode = s.CurrencyCode,
                UpdateDate = s.UpdateDate
            }).ToList();
        }

    }
}

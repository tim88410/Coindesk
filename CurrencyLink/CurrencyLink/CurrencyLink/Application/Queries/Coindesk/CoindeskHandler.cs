using CurrencyLink.Infrastructure.Models.Coindesk;
using CurrencyLink.Infrastructure.Repositories.Coindesk;
using MediatR;

namespace CurrencyLink.Application.Queries.Coindesk
{
    public class CoindeskHandler : IRequestHandler<CoindeskRequest, CoindeskResponse?>
    {
        private readonly ICoindeskQueryRepository _coindeskQueryRepository;
        public CoindeskHandler(ICoindeskQueryRepository coindeskQueryRepository) 
        {
            _coindeskQueryRepository = coindeskQueryRepository;
        }

        public async Task<CoindeskResponse?> Handle(CoindeskRequest request, CancellationToken cancellationToken)
        {
            CoindeskQuery.CoindeskQueryParameter coindeskParameter = new CoindeskQuery.CoindeskQueryParameter()
            {
                Code = request.Code,
                CodeName = request.CodeName,
                Page = request.Page,
                PageLimit = request.PageLimit,
                SortOrderBy = request.SortOrderBy,
                SortColumn = request.SortColumn
            };
            var coindeskQuery = await _coindeskQueryRepository.GetAsync(coindeskParameter);
            //DBConnectError
            if (coindeskQuery == null) { 
                return null;
            }
            return new CoindeskResponse {
                CoindeskInfos = coindeskQuery.Select(s => new CoindeskResponse.CoindeskInfo() {
                    Id = s.Id,
                    Code = s.Code,
                    CodeName = s.CodeName,
                    Symbol = s.Symbol,
                    Description = s.Description,
                    Rate = s.Rate,
                    RateFloat = s.Rate_float,
                    CurrencyCode = s.CurrencyCode,
                    UpdateDate = s.UpdateDate
                }).ToList(),
                Total = coindeskQuery.Select(s => s.TotalItem).FirstOrDefault()
            };
        }
    }
}

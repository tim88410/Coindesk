using MediatR;

namespace CurrencyLink.Application.Queries.Coindesk
{
    public class CoindeskGetOneRequest : IRequest<IEnumerable<CoindeskResponse.CoindeskInfo>?>
    {
        /// <summary>
        /// 查詢對象流水號
        /// </summary>
        public int Id { get; set; }
    }
}

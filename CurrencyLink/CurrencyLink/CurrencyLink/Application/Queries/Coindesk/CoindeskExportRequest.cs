using MediatR;

namespace CurrencyLink.Application.Queries.Coindesk
{
    public record CoindeskExportRequest : CoindeskRequest, IRequest<CoindeskExportResponse?>
    {
    }
}

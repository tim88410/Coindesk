using MediatR;

namespace CurrencyLink.Application.Commands.Coindesk
{
    public class DeleteCoindeskCommand :IRequest<int>
    {
        public int Id { get; set; }
    }
}

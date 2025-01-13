using AutoMapper;
using CurrencyLink.Application.Commands.Coindesk;
using CurrencyLink.Infrastructure.Models.Coindesk;

namespace CurrencyLink.Application.Commands
{
    public class CommandProfile : Profile
    {
        public CommandProfile()
        {
            CreateMap<UpdateCoindeskCommand, CoindeskCommand.CoindeskParameter>()
                .ForMember(
                    src => src.Rate_float,
                    target => target.MapFrom(x => x.RateFloat)
                ).ReverseMap();
        }
    }
}

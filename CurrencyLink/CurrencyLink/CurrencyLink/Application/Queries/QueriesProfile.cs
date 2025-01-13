using AutoMapper;
using CurrencyLink.Application.Queries.Coindesk;
using CurrencyLink.Infrastructure.Models.Coindesk;

namespace CurrencyLink.Application.Queries
{
    public class QueriesProfile : Profile
    {
        public QueriesProfile()
        {
            CreateMap<CoindeskResponse.CoindeskInfo, CoindeskQuery.CoindeskDTO>()
                .ForMember(
                    src => src.Rate_float,
                    target => target.MapFrom(x => x.RateFloat)
                ).ReverseMap();
            CreateMap<CoindeskRequest, CoindeskQuery.CoindeskQueryParameter>().ReverseMap();
        }
    }
}

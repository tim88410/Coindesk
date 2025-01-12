using CurrencyLink.Domain.Repositories.CoindeskAPIClient;
using CurrencyLink.Domain.Service;
using DBUtility;
using MediatR;
using System.Text.Json;

namespace CurrencyLink.Application.Commands.CoindeskAPIClient
{
    public class CoindeskAPIClientHandler : IRequestHandler<CoindeskAPIClientCommand, int>
    {
        private readonly ICoindeskAPIClientService _coindeskService;
        private readonly ICoindeskAPIClientRepository _coindeskRepository;
        public CoindeskAPIClientHandler(ICoindeskAPIClientService coindeskService, ICoindeskAPIClientRepository coindeskRepository)
        {
            _coindeskService = coindeskService;
            _coindeskRepository = coindeskRepository;
        }

        public async Task<int> Handle(CoindeskAPIClientCommand command, CancellationToken cancellationToken)
        {
            var result = _coindeskService.GetCoindesk();
            int updateRet;
            if (result.nRet == ErrorCode.KErrNone)
            {
                string json = JsonSerializer.Serialize(result.coindeskDTO);
                updateRet = await _coindeskRepository.ApiUpdate(json);
            }
            return result.nRet;
        }
    }
}

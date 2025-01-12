using CurrencyLink.Domain.Models;

namespace CurrencyLink.Domain.Service
{
    public interface ICoindeskAPIClientService
    {
        public (int nRet, CoindeskClientDTO coindeskDTO) GetCoindesk();
    }
}

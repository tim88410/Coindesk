namespace CurrencyLink.Domain.Repositories.CoindeskAPIClient
{
    public interface ICoindeskAPIClientRepository
    {
        public Task<int> ApiUpdate(string currencyUpdateParameter);
    }
}

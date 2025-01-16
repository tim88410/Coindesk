namespace CurrencyLink.Application.Queries.Coindesk
{
    public class CoindeskExportResponse
    {
        public List<CoindeskResponse.CoindeskInfo> CoindeskInfo { get; set; } = new List<CoindeskResponse.CoindeskInfo>();
    }
}

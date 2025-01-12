using CurrencyLink.Domain.Repositories.CoindeskAPIClient;
using DBUtility;
using System.Data.SqlClient;

namespace CurrencyLink.Infrastructure.Repositories.CoindeskAPIClient
{
    public class CoindeskAPIClientRepository : ICoindeskAPIClientRepository
    {
        private readonly IDataBaseUtility _dataBaseUtility;

        public CoindeskAPIClientRepository(IDataBaseUtility dataBaseUtility)
        {
            _dataBaseUtility = dataBaseUtility;
        }

        public async Task<int> ApiUpdate(string currencyUpdateParameter)
        {
            string sql = @"
MERGE INTO CoinRate AS T
USING (
    SELECT 
		Parameter.UpdatedISO,
		currencyCode AS CurrencyCode,
		code AS Code,
		NULL AS CodeName,
		symbol AS Symbol,
		description AS Description,
		rate_float AS Rate_float,
		rate AS Rate
	FROM
		OPENJSON (@CurrencyUpdateParameter)  
		WITH (   
			UpdatedISO DATETIME2,
			CurrencyInfos NVARCHAR(MAX) AS JSON
		) AS Parameter
		CROSS APPLY OPENJSON(Parameter.CurrencyInfos) WITH (
			currencyCode VARCHAR(20),
			code VARCHAR(20),
			symbol VARCHAR(20),
			description NVARCHAR(1000),
			rate_float DECIMAL(18,4),
			rate VARCHAR(20)
		) AS PromoMedia
) AS S ON T.CurrencyCode = S.CurrencyCode
WHEN MATCHED THEN
    UPDATE SET
        T.Code = S.Code, 
        T.Symbol = S.Symbol, 
        T.Description = S.Description, 
        T.Rate_float = S.Rate_float, 
        T.Rate = S.Rate,
		T.UpdateDate = S.UpdatedISO
WHEN NOT MATCHED THEN
    INSERT (CurrencyCode, Code, CodeName, Symbol, Description, Rate_float, Rate, UpdateDate)
    VALUES (S.CurrencyCode, S.Code, S.CodeName, S.Symbol, S.Description, S.Rate_float, S.Rate, S.UpdatedISO);
";
            List<SqlParameter> sqlParameter = new()
            {
                new SqlParameter("CurrencyUpdateParameter", System.Data.SqlDbType.NVarChar) { Value = currencyUpdateParameter }
            };

            return await _dataBaseUtility.UpdateAsync(sql, sqlParameter);
        }
    }
}

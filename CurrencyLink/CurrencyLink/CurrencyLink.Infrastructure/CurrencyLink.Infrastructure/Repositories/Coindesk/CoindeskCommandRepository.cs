using CurrencyLink.Infrastructure.Models.Coindesk;
using DBUtility;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyLink.Infrastructure.Repositories.Coindesk
{
    public interface ICoindeskCommandRepository
    {
        public Task<int> UpdateAsync(CoindeskCommand.CoindeskParameter request);
        public Task<int> DeleteAsync(CoindeskCommand.CoindeskDeleteParameter request);
    }

    public class CoindeskCommandRepository : ICoindeskCommandRepository
    {
        private readonly IDataBaseUtility _dataBaseUtility;

        public CoindeskCommandRepository(IDataBaseUtility dataBaseUtility)
        {
            _dataBaseUtility = dataBaseUtility;
        }

        public async Task<int> UpdateAsync(CoindeskCommand.CoindeskParameter request)
        {
            string sql = @"
MERGE INTO CoinRate AS T
USING (
    SELECT 
		@Id AS Id,
		@Code AS Code,
		@CodeName AS CodeName,
		@Symbol AS Symbol,
		@Description AS Description,
		@Rate_float AS Rate_float,
		@Rate AS Rate,
		@CurrencyCode AS CurrencyCode
) AS S ON T.Id = S.Id
WHEN MATCHED THEN
    UPDATE SET
        T.Code = S.Code, 
        T.CodeName = S.CodeName, 
        T.Symbol = S.Symbol, 
        T.Description = S.Description, 
        T.Rate_float = S.Rate_float, 
        T.Rate = S.Rate,
		T.CurrencyCode = S.CurrencyCode,
		T.UpdateDate = GETDATE()
WHEN NOT MATCHED THEN
    INSERT (CurrencyCode, Code, CodeName, Symbol, Description, Rate_float, Rate, UpdateDate)
    VALUES (S.CurrencyCode, S.Code, S.CodeName, S.Symbol, S.Description, S.Rate_float, S.Rate, GETDATE());
";
            List<SqlParameter> sqlParameter = new()
            {
                new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = request.Id },
                new SqlParameter("Code", System.Data.SqlDbType.VarChar) { Value = request.Code },
                new SqlParameter("CodeName", System.Data.SqlDbType.NVarChar) { Value = request.CodeName },
                new SqlParameter("Symbol", System.Data.SqlDbType.VarChar) { Value = request.Symbol },
                new SqlParameter("Description", System.Data.SqlDbType.NVarChar) { Value = request.Description },
                new SqlParameter("Rate_float", System.Data.SqlDbType.Decimal) { Value = request.Rate_float },
                new SqlParameter("Rate", System.Data.SqlDbType.VarChar) { Value = request.Rate },
                new SqlParameter("CurrencyCode", System.Data.SqlDbType.VarChar) { Value = request.CurrencyCode }
            };

            return await _dataBaseUtility.UpdateAsync(sql, sqlParameter);
        }

        public async Task<int> DeleteAsync(CoindeskCommand.CoindeskDeleteParameter request)
        {
            string sql = @"
DELETE FROM CoinRate WHERE Id = @Id
";
            List<SqlParameter> sqlParameter = new()
            {
                new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = request.Id }
            };

            return await _dataBaseUtility.DeleteAsync(sql, sqlParameter);
        }
    }
}

using CurrencyLink.Infrastructure.Models.Coindesk;
using DBUtility;
using System.Data;
using System.Data.SqlClient;

namespace CurrencyLink.Infrastructure.Repositories.Coindesk
{
    public interface ICoindeskQueryRepository
    {
        public Task<IEnumerable<CoindeskQuery.CoindeskDTO>> GetAsync(CoindeskQuery.CoindeskQueryParameter request);
        public Task<IEnumerable<CoindeskQuery.CoindeskDTO>?> GetOneAsync(int Id);
    }

    public class CoindeskQueryRepository : ICoindeskQueryRepository
    {
        private readonly IDataBaseUtility _dataBaseUtility;

        public CoindeskQueryRepository(IDataBaseUtility dataBaseUtility)
        {
            _dataBaseUtility = dataBaseUtility;
        }

        public async Task<IEnumerable<CoindeskQuery.CoindeskDTO>> GetAsync(CoindeskQuery.CoindeskQueryParameter request)
        {
            string sql = @"
WITH RateSort AS
(                               
    SELECT 
        Id,
		Code,
		CodeName,
		Symbol,
		Description,
		Rate_float,
		Rate,
		CurrencyCode,
		UpdateDate, 
        CASE @SortBy
			WHEN 'Id ASC' THEN ROW_NUMBER() OVER (ORDER BY Id)
			WHEN 'Id DESC' THEN ROW_NUMBER() OVER (ORDER BY Id DESC)
			WHEN 'Code ASC' THEN ROW_NUMBER() OVER (ORDER BY Code)
			WHEN 'Code DESC' THEN ROW_NUMBER() OVER (ORDER BY Code DESC)
			WHEN 'CodeName ASC' THEN ROW_NUMBER() OVER (ORDER BY CodeName)
			WHEN 'CodeName DESC' THEN ROW_NUMBER() OVER (ORDER BY CodeName DESC)
			WHEN 'Symbol ASC' THEN ROW_NUMBER() OVER (ORDER BY Symbol)
			WHEN 'Symbol DESC' THEN ROW_NUMBER() OVER (ORDER BY Symbol DESC)
			WHEN 'Description ASC' THEN ROW_NUMBER() OVER (ORDER BY Description)
			WHEN 'Description DESC' THEN ROW_NUMBER() OVER (ORDER BY Description DESC)
			WHEN 'Rate_float ASC' THEN ROW_NUMBER() OVER (ORDER BY Rate_float)
			WHEN 'Rate_float DESC' THEN ROW_NUMBER() OVER (ORDER BY Rate_float DESC)
			WHEN 'Rate ASC' THEN ROW_NUMBER() OVER (ORDER BY Rate)
			WHEN 'Rate DESC' THEN ROW_NUMBER() OVER (ORDER BY Rate DESC)
			WHEN 'UpdateDate ASC' THEN ROW_NUMBER() OVER (ORDER BY UpdateDate)
			WHEN 'UpdateDate DESC' THEN ROW_NUMBER() OVER (ORDER BY UpdateDate DESC)
		END AS RowNo
    FROM
        (
            SELECT
                Id,
				Code,
				CodeName,
				Symbol,
				Description,
				Rate_float,
				Rate,
				CurrencyCode,
				UpdateDate
            FROM
                CoinRate
            WHERE
                (@Code = '' OR CoinRate.Code = @Code) AND
                (@CodeName = '' OR CoinRate.CodeName = @CodeName)
		) RateSource
), RateSourceCount AS (SELECT COUNT(1) AS TotalItem FROM RateSort)

SELECT     
	Id,
	Code,
	CodeName,
	Symbol,
	Description,
	Rate_float,
	Rate,
	CurrencyCode,
	UpdateDate,
	TotalItem
FROM        
	RateSort, RateSourceCount
WHERE 
	RowNo between @StartIndex and @EndIndex
ORDER BY 
	RowNo
";
            List<SqlParameter> sqlParams = new List<SqlParameter>
            {
                new SqlParameter { ParameterName = "Code", Value = request.Code ?? string.Empty, SqlDbType = SqlDbType.VarChar },
                new SqlParameter { ParameterName = "CodeName", Value = request.CodeName ?? string.Empty, SqlDbType = SqlDbType.NVarChar },
                new SqlParameter { ParameterName = "StartIndex", Value = request.StartIndex, SqlDbType = SqlDbType.Int },
                new SqlParameter { ParameterName = "EndIndex", Value = request.EndIndex, SqlDbType = SqlDbType.Int },
                new SqlParameter { ParameterName = "SortBy", Value = request.SortBy ?? string.Empty, SqlDbType = SqlDbType.VarChar }
            };

            return await _dataBaseUtility.QueryAsync<CoindeskQuery.CoindeskDTO>(sql, sqlParams);
        }
        public async Task<IEnumerable<CoindeskQuery.CoindeskDTO>?> GetOneAsync(int Id)
        {
            string sql = @"
SELECT     
	Id,
	Code,
	CodeName,
	Symbol,
	Description,
	Rate_float,
	Rate,
	CurrencyCode,
	UpdateDate
FROM        
	CoinRate
WHERE 
	Id = @Id
";
            List<SqlParameter> sqlParams = new List<SqlParameter>
            {
                new SqlParameter { ParameterName = "Id", Value = Id, SqlDbType = SqlDbType.Int }
            };

            return await _dataBaseUtility.QueryAsync<CoindeskQuery.CoindeskDTO>(sql, sqlParams);
        }
    }
}

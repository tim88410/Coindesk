using CurrencyLink.Infrastructure.Models.Coindesk;
using CurrencyLink.Infrastructure.Repositories.Coindesk;
using DBUtility;
using Moq;
using System.Data;
using System.Data.SqlClient;

namespace CurrencyLink.Tests.Repositories.Coindesk
{
    public class CoindeskCommandRepositoryTest
    {
        private readonly Mock<IDataBaseUtility> _mockDataBaseUtility;
        private readonly CoindeskCommandRepository _repository;

        public CoindeskCommandRepositoryTest()
        {
            _mockDataBaseUtility = new Mock<IDataBaseUtility>();
            _repository = new CoindeskCommandRepository(_mockDataBaseUtility.Object);
        }

        [Fact]
        public async Task UpdateAsync_Successful()
        {
            // Arrange
            var request = new CoindeskCommand.CoindeskParameter
            {
                Id = 1,
                Code = "BTC",
                CodeName = "Bitcoin",
                Symbol = "BTC",
                Description = "Bitcoin Cryptocurrency",
                Rate_float = 60000.50m,
                Rate = "60000.50",
                CurrencyCode = "USD"
            };

            var sql = @"
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

            _mockDataBaseUtility
                .Setup(db => db.UpdateAsync(sql, It.IsAny<List<SqlParameter>>(), CommandType.Text))
                .ReturnsAsync(ErrorCode.KErrNone);

            // Act
            var result = await _repository.UpdateAsync(request);

            // Assert
            Assert.Equal(ErrorCode.KErrNone, result);
        }

        [Fact]
        public async Task DeleteAsync_Successful()
        {
            // Arrange
            var request = new CoindeskCommand.CoindeskDeleteParameter
            {
                Id = 1
            };

            var sql = "DELETE FROM CoinRate WHERE Id = @Id";

            _mockDataBaseUtility
                .Setup(db => db.DeleteAsync(sql, It.IsAny<List<SqlParameter>>(), CommandType.Text))
                .ReturnsAsync(ErrorCode.KErrNone);

            // Act
            var result = await _repository.DeleteAsync(request);

            // Assert
            Assert.Equal(ErrorCode.KErrNone, result);
        }
    }
}
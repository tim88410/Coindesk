using CurrencyLink.Infrastructure.Models.Coindesk;
using CurrencyLink.Infrastructure.Repositories.Coindesk;
using DBUtility;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CurrencyLink.Infrastructure.Models.Coindesk.CoindeskQuery;

namespace CurrencyLink.Infrastructure.Test.Repositories.Coindesk
{
    public class CoindeskQueryRepositoryTest
    {
        private readonly Mock<IDataBaseUtility> _mockDataBaseUtility;
        private readonly CoindeskQueryRepository _repository;

        public CoindeskQueryRepositoryTest()
        {
            _mockDataBaseUtility = new Mock<IDataBaseUtility>();
            _repository = new CoindeskQueryRepository(_mockDataBaseUtility.Object);
        }

        [Fact]
        public async Task GetAsync_ValidRequest_ReturnsData()
        {
            // Arrange
            var fakeRequest = new CoindeskQuery.CoindeskQueryParameter
            {
                Code = "USD",
                CodeName = "United States Dollar",
                Page = 1,
                PageLimit = 10,
                SortColumn = "Code",
                SortOrderBy = "ASC"
            };

            var fakeResponse = new List<CoindeskQuery.CoindeskDTO>
            {
                new CoindeskQuery.CoindeskDTO
                {
                    Id = 1,
                    Code = "USD",
                    CodeName = "United States Dollar",
                    Symbol = "$",
                    Description = "US Dollar",
                    Rate_float = (decimal)20000.00f,
                    Rate = "20,000.00",
                    CurrencyCode = "USD"
                }
            };

            _mockDataBaseUtility
                .Setup(db => db.QueryAsync<CoindeskQuery.CoindeskDTO>(
                    It.IsAny<string>(),
                    It.IsAny<List<SqlParameter>>(), CommandType.Text))
                .ReturnsAsync(fakeResponse);

            // Act
            var result = await _repository.GetAsync(fakeRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("USD", result.First().Code);
        }

        [Fact]
        public async Task GetOneAsync_ValidId_ReturnsSingleData()
        {
            // Arrange
            int id = 1;
            var fakeResponse = new List<CoindeskQuery.CoindeskDTO>
        {
            new CoindeskQuery.CoindeskDTO
            {
                Id = 1,
                Code = "USD",
                CodeName = "United States Dollar",
                Symbol = "$",
                Description = "US Dollar",
                Rate_float = (decimal)20000.00f,
                Rate = "20,000.00",
                CurrencyCode = "USD"
            }
        };

            _mockDataBaseUtility
                .Setup(db => db.QueryAsync<CoindeskQuery.CoindeskDTO>(
                    It.IsAny<string>(),
                    It.Is<List<SqlParameter>>(parameters =>
                        parameters.Any(p => p.ParameterName == "Id" && (int)p.Value == id)), CommandType.Text))
                .ReturnsAsync(fakeResponse);

            // Act
            var result = await _repository.GetOneAsync(id);

            // Assert
            Assert.NotNull(result);
            var item = result.First();
            Assert.Equal(id, item.Id);
            Assert.Equal("USD", item.Code);
        }

        [Fact]
        public async Task GetOneAsync_InvalidId_ReturnsNull()
        {
            // Arrange
            int invalidId = 999;
            _mockDataBaseUtility
                .Setup(db => db.QueryAsync<CoindeskQuery.CoindeskDTO>(
                    It.IsAny<string>(),
                    It.IsAny<List<SqlParameter>>(), CommandType.Text))
                .ReturnsAsync((IEnumerable<CoindeskQuery.CoindeskDTO>?)null);

            // Act
            var result = await _repository.GetOneAsync(invalidId);

            // Assert
            Assert.Null(result);
        }
    }
}

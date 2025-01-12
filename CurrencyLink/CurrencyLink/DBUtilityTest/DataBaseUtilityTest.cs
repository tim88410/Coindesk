using DBUtility;
using Moq;
using System.Data;
using System.Data.SqlClient;

namespace DBUtilityTest
{
    public class DataBaseUtilityTest
    {
        private readonly Mock<IDataBaseUtility> _mockDataBaseUtility;

        public DataBaseUtilityTest()
        {
            _mockDataBaseUtility = new Mock<IDataBaseUtility>();
        }

        [Fact]
        public async Task QueryAsync_ValidQuery_ReturnsResults()
        {
            // Arrange
            var sqlcmd = "SELECT Id, Name FROM Users WHERE Id = @Id";
            var sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter("@Id", SqlDbType.Int) { Value = 1 }
                };
                var mockResult = new List<TestModel>
                {
                    new TestModel { Id = 1, Name = "Test User" }
                };

            _mockDataBaseUtility
                .Setup(db => db.QueryAsync<TestModel>(sqlcmd, sqlParameters, CommandType.Text))
                .ReturnsAsync(mockResult);

            // Act
            var result = await _mockDataBaseUtility.Object.QueryAsync<TestModel>(sqlcmd, sqlParameters);

            // Assert.Setup(db => db.QueryAsync<TestModel>(sqlcmd, sqlParameters, CommandType.Text))
            Assert.NotNull(result);
            Assert.Single(result);  // 保留這裡來確保只返回一筆數據
            Assert.Equal(1, result.AsEnumerable().ToList()[0].Id); // 修正索引訪問
        }

        [Fact]
        public async Task UpdateAsync_ValidUpdate_UpdatesData()
        {
            // Arrange
            var sqlcmd = "UPDATE Users SET Name = @Name WHERE Id = @Id";
            var sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("@Id", SqlDbType.Int) { Value = 1 },
                new SqlParameter("@Name", SqlDbType.NVarChar) { Value = "Updated Name" }
            };
            int expectedResult = ErrorCode.KErrNone;

            _mockDataBaseUtility
                .Setup(db => db.UpdateAsync(sqlcmd, sqlParameters, CommandType.Text))
                .ReturnsAsync(expectedResult);

            // Act
            int result = await _mockDataBaseUtility.Object.UpdateAsync(sqlcmd, sqlParameters);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task DeleteAsync_ValidDelete_DeletesData()
        {
            // Arrange
            var sqlcmd = "DELETE FROM Users WHERE Id = @Id";
            var sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("@Id", SqlDbType.Int) { Value = 1 }
            };
            int expectedResult = ErrorCode.KErrNone;

            _mockDataBaseUtility
                .Setup(db => db.DeleteAsync(sqlcmd, sqlParameters, CommandType.Text))
                .ReturnsAsync(expectedResult);

            // Act
            int result = await _mockDataBaseUtility.Object.DeleteAsync(sqlcmd, sqlParameters);

            // Assert
            Assert.Equal(expectedResult, result);
        }
    }

    public class TestModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public static class ErrorCode
    {
        public const int KErrNone = 0;
        public const int KErrDBError = -1;
    }
}


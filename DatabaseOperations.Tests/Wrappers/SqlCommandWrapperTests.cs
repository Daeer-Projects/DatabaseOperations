using System.Data;
using DatabaseOperations.Interfaces;
using DatabaseOperations.Wrappers;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using NSubstitute;
using Xunit;

namespace DatabaseOperations.Tests.Wrappers
{
    public class SqlCommandWrapperTests
    {
        [Fact]
        public void TestGetReturnsNonNullCommand()
        {
            // Arrange.
            var sqlConnection = Substitute.For<ISqlConnectionWrapper>();
            var sqlCommand = new SqlCommandWrapper("SELECT '0';", sqlConnection);

            // Act.
            var actualCommand = sqlCommand.Get();

            // Assert.
            actualCommand.Should().NotBeNull();
        }

        [Fact]
        public void TestSetCommandTimeoutReturnsSetsTimeout()
        {
            // Arrange.
            var sqlConnection = Substitute.For<ISqlConnectionWrapper>();
            var sqlCommand = new SqlCommandWrapper("SELECT '0';", sqlConnection);
            sqlCommand.SetCommandTimeout(27);

            // Act.
            var actualCommand = sqlCommand.Get();

            // Assert.
            actualCommand.CommandTimeout.Should().Be(27);
        }

        [Fact]
        public void TestAddParametersAddsExpected()
        {
            // Arrange.
            var sqlConnection = Substitute.For<ISqlConnectionWrapper>();
            var sqlCommand = new SqlCommandWrapper("SELECT '0';", sqlConnection);
            SqlParameter[] parameters =
            {
                new("@First", SqlDbType.VarChar)
                {
                    Value = "Name"
                },
                new("@Second", SqlDbType.VarChar)
                {
                    Value = "Thing"
                }
            };

            sqlCommand.AddParameters(parameters);

            // Act.
            var actual = sqlCommand.Get();

            // Assert.
            actual.Parameters.Count.Should().Be(2);
            for (var i = 0; i < actual.Parameters.Count; i++)
            {
                actual.Parameters[i].Value.Should().BeEquivalentTo(parameters[i].Value);
            }
        }
    }
}

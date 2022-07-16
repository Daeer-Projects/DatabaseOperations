namespace DatabaseOperations.Tests.Wrappers
{
    using System.Data;
    using DatabaseOperations.Wrappers;
    using FluentAssertions;
    using Interfaces;
    using Microsoft.Data.SqlClient;
    using NSubstitute;
    using Xunit;

    public class SqlCommandWrapperTests
    {
        [Fact]
        public void TestGetReturnsNonNullCommand()
        {
            // Arrange.
            ISqlConnectionWrapper? sqlConnection = Substitute.For<ISqlConnectionWrapper>();
            SqlCommandWrapper sqlCommand = new SqlCommandWrapper("SELECT '0';", sqlConnection);

            // Act.
            SqlCommand actualCommand = sqlCommand.Get();

            // Assert.
            actualCommand.Should()
                .NotBeNull();
        }

        [Fact]
        public void TestSetCommandTimeoutReturnsSetsTimeout()
        {
            // Arrange.
            ISqlConnectionWrapper? sqlConnection = Substitute.For<ISqlConnectionWrapper>();
            SqlCommandWrapper sqlCommand = new SqlCommandWrapper("SELECT '0';", sqlConnection);
            sqlCommand.SetCommandTimeout(27);

            // Act.
            SqlCommand actualCommand = sqlCommand.Get();

            // Assert.
            actualCommand.CommandTimeout.Should()
                .Be(27);
        }

        [Fact]
        public void TestAddParametersAddsExpected()
        {
            // Arrange.
            ISqlConnectionWrapper? sqlConnection = Substitute.For<ISqlConnectionWrapper>();
            SqlCommandWrapper sqlCommand = new SqlCommandWrapper("SELECT '0';", sqlConnection);
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
            SqlCommand actual = sqlCommand.Get();

            // Assert.
            actual.Parameters.Count.Should()
                .Be(2);
            for (int i = 0; i < actual.Parameters.Count; i++)
                actual.Parameters[i]
                    .Value.Should()
                    .BeEquivalentTo(
                        parameters[i]
                            .Value);
        }
    }
}
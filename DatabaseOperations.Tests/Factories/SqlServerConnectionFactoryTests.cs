using DatabaseOperations.Factories;
using DatabaseOperations.Interfaces;
using DatabaseOperations.Wrappers;
using FluentAssertions;
using Xunit;

namespace DatabaseOperations.Tests.Factories
{
    public class SqlServerConnectionFactoryTests
    {
        private readonly ISqlServerConnectionFactory _connectionFactory = new SqlServerConnectionFactory();

        [Fact]
        public void TestCreateConnectionReturnsExpected()
        {
            // Arrange.
            // Act.
            var connection = _connectionFactory.CreateConnection("server=127.0.0.1;database=Thing;");

            // Assert.
            connection.Should().NotBeNull();
            connection.Should().BeOfType<SqlConnectionWrapper>();
        }

        [Fact]
        public void TestCreateCommandReturnsExpected()
        {
            // Arrange.
            // Act.
            var command = _connectionFactory.CreateCommand("SELECT '0';", new SqlConnectionWrapper("server=127.0.0.1;database=Thing;"));

            // Assert.
            command.Should().NotBeNull();
            command.Should().BeOfType<SqlCommandWrapper>();
        }
    }
}

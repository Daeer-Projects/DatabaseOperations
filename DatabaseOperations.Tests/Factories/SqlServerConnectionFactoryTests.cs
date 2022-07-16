namespace DatabaseOperations.Tests.Factories
{
    using DatabaseOperations.Factories;
    using DatabaseOperations.Wrappers;
    using FluentAssertions;
    using Interfaces;
    using Xunit;

    public class SqlServerConnectionFactoryTests
    {
        private readonly ISqlServerConnectionFactory connectionFactory = new SqlServerConnectionFactory();

        [Fact]
        public void TestCreateConnectionReturnsExpected()
        {
            // Arrange.
            // Act.
            ISqlConnectionWrapper connection = connectionFactory.CreateConnection("server=127.0.0.1;database=Thing;");

            // Assert.
            connection.Should()
                .NotBeNull();
            connection.Should()
                .BeOfType<SqlConnectionWrapper>();
        }

        [Fact]
        public void TestCreateCommandReturnsExpected()
        {
            // Arrange.
            // Act.
            ISqlCommandWrapper command = connectionFactory.CreateCommand(
                "SELECT '0';",
                new SqlConnectionWrapper("server=127.0.0.1;database=Thing;"));

            // Assert.
            command.Should()
                .NotBeNull();
            command.Should()
                .BeOfType<SqlCommandWrapper>();
        }
    }
}
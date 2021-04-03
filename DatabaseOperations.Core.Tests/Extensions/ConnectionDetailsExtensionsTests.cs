using System;
using DatabaseOperations.Core.DataTransferObjects;
using DatabaseOperations.Core.Extensions;
using FluentAssertions;
using Xunit;

namespace DatabaseOperations.Core.Tests.Extensions
{
    public class ConnectionDetailsExtensionsTests
	{
        [Theory]
        [InlineData("(localdb)")]
        [InlineData(".\\")]
        public void TestIsLocalWithValidLocalServersReturnsTrue(string serverName)
        {
            // Arrange.
            ConnectionDetails details = new ConnectionDetails
            {
                ServerName = serverName
            };

            // Act.
            bool result = details.IsLocalServer();

            // Assert.
            result.Should().BeTrue();
        }

        [Fact]
        public void TestIsLocalWithValidLocalServerMachineReturnsTrue()
        {
            // Arrange.
            ConnectionDetails details = new ConnectionDetails
            {
                ServerName = Environment.MachineName
            };

            // Act.
            bool result = details.IsLocalServer();

            // Assert.
            result.Should().BeTrue();
        }

        [Theory]
        [InlineData("192.168.16.999")]
        [InlineData("127.0.0.1")]
        [InlineData("Machine_01")]
        public void TestIsLocalWithValidRemoteServersReturnsFalse(string serverName)
        {
            // Arrange.
            ConnectionDetails details = new ConnectionDetails
            {
                ServerName = serverName
            };

            // Act.
            bool result = details.IsLocalServer();

            // Assert.
            result.Should().BeFalse();
        }
	}
}

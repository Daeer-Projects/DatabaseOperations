namespace DatabaseOperations.Tests.Validators
{
    using System.Collections.Generic;
    using DatabaseOperations.DataTransferObjects;
    using FluentAssertions;
    using Xunit;

    public class ConnectionOptionsValidatorTests
    {
        private const string BackupPath = @"C:\Database Backups\";

        [Fact]
        public void TestIsValidWithValidOptionsReturnsTrue()
        {
            // Arrange.
            ConnectionOptions connection = new ConnectionOptions("something", BackupPath, 5)
                .ApplyServer("127.0.0.1")
                .ApplyDatabaseName("Bananas")
                .ApplyUserId("sa")
                .ApplyPassword("password")
                .ApplyConnectTimeOut("205");

            // Act.
            bool result = connection.IsValid();

            // Assert.
            result.Should()
                .BeTrue();
        }

        [Fact]
        public void TestIsValidWithSecuritySetAndMissingUserReturnsTrue()
        {
            // Arrange.
            ConnectionOptions connection = new ConnectionOptions("something", BackupPath, 5)
                .ApplyServer("127.0.0.1")
                .ApplyDatabaseName("Bananas")
                .ApplyIntegratedSecurity("SSPI")
                .ApplyConnectTimeOut("205");

            // Act.
            bool result = connection.IsValid();

            // Assert.
            result.Should()
                .BeTrue();
        }

        [Fact]
        public void TestIsValidWithMissingServerReturnsFalse()
        {
            // Arrange.
            ConnectionOptions connection = new ConnectionOptions("something", BackupPath, 5)
                .ApplyDatabaseName("Bananas")
                .ApplyUserId("sa")
                .ApplyPassword("password")
                .ApplyConnectTimeOut("205");

            // Act.
            bool result = connection.IsValid();

            // Assert.
            result.Should()
                .BeFalse();
        }

        [Fact]
        public void TestIsValidWithMissingDatabaseReturnsFalse()
        {
            // Arrange.
            ConnectionOptions connection = new ConnectionOptions("something", BackupPath, 5)
                .ApplyServer("127.0.0.1")
                .ApplyUserId("sa")
                .ApplyPassword("password")
                .ApplyConnectTimeOut("205");

            // Act.
            bool result = connection.IsValid();

            // Assert.
            result.Should()
                .BeFalse();
        }

        [Fact]
        public void TestIsValidWithMissingUserAndSecurityReturnsFalse()
        {
            // Arrange.
            ConnectionOptions connection = new ConnectionOptions("something", BackupPath, 5)
                .ApplyServer("127.0.0.1")
                .ApplyDatabaseName("Bananas")
                .ApplyConnectTimeOut("205");

            // Act.
            bool result = connection.IsValid();

            // Assert.
            result.Should()
                .BeFalse();
        }

        [Theory]
        [MemberData(nameof(ConnectionOptions))]
        public void TestIsValidWithInvalidOptionsReturnsFalse(ConnectionOptions options)
        {
            // Arrange.
            // Act.
            bool result = options.IsValid();

            // Assert.
            result.Should()
                .BeFalse();
        }

        public static IEnumerable<object[]> ConnectionOptions()
        {
            yield return new object[]
            {
                new ConnectionOptions("something", BackupPath, 5)
                    .ApplyServer("")
                    .ApplyDatabaseName("Bananas")
                    .ApplyUserId("sa")
                    .ApplyPassword("password")
                    .ApplyConnectTimeOut("205")
            };
            yield return new object[]
            {
                new ConnectionOptions("something", BackupPath, 5)
                    .ApplyServer("    ")
                    .ApplyDatabaseName("Bananas")
                    .ApplyUserId("sa")
                    .ApplyPassword("password")
                    .ApplyConnectTimeOut("205")
            };
            yield return new object[]
            {
                new ConnectionOptions("something", BackupPath, 5)
                    .ApplyServer("(localDb)")
                    .ApplyDatabaseName("")
                    .ApplyUserId("sa")
                    .ApplyPassword("password")
                    .ApplyConnectTimeOut("205")
            };
            yield return new object[]
            {
                new ConnectionOptions("something", BackupPath, 5)
                    .ApplyServer("(localDb)")
                    .ApplyDatabaseName("    ")
                    .ApplyUserId("sa")
                    .ApplyPassword("password")
                    .ApplyConnectTimeOut("205")
            };
            yield return new object[]
            {
                new ConnectionOptions("something", BackupPath, 5)
                    .ApplyServer("(localDb)")
                    .ApplyDatabaseName("Banana")
                    .ApplyUserId("")
                    .ApplyPassword("password")
                    .ApplyConnectTimeOut("205")
            };
            yield return new object[]
            {
                new ConnectionOptions("something", BackupPath, 5)
                    .ApplyServer("(localDb)")
                    .ApplyDatabaseName("Banana")
                    .ApplyUserId("  ")
                    .ApplyPassword("password")
                    .ApplyConnectTimeOut("205")
            };
            yield return new object[]
            {
                new ConnectionOptions("something", BackupPath, 5)
                    .ApplyServer("(localDb)")
                    .ApplyDatabaseName("Banana")
                    .ApplyUserId("Pooh")
                    .ApplyPassword("")
                    .ApplyConnectTimeOut("205")
            };
            yield return new object[]
            {
                new ConnectionOptions("something", BackupPath, 5)
                    .ApplyServer("(localDb)")
                    .ApplyDatabaseName("Banana")
                    .ApplyUserId("Pooh")
                    .ApplyPassword("\r\n")
                    .ApplyConnectTimeOut("205")
            };
            yield return new object[]
            {
                new ConnectionOptions("something", BackupPath, 5)
                    .ApplyServer("(localDb)")
                    .ApplyDatabaseName("Banana")
                    .ApplyIntegratedSecurity("yes")
                    .ApplyConnectTimeOut("205")
            };
            yield return new object[]
            {
                new ConnectionOptions("something", BackupPath, 5)
                    .ApplyServer("(localDb)")
                    .ApplyDatabaseName("Banana")
                    .ApplyIntegratedSecurity("no")
                    .ApplyConnectTimeOut("205")
            };
            yield return new object[]
            {
                new ConnectionOptions("something", BackupPath, 5)
                    .ApplyServer("(localDb)")
                    .ApplyDatabaseName("Banana")
                    .ApplyIntegratedSecurity("nope")
                    .ApplyConnectTimeOut("205")
            };
            yield return new object[]
            {
                new ConnectionOptions("something", BackupPath, 5)
                    .ApplyServer("(localDb)")
                    .ApplyDatabaseName("Banana")
                    .ApplyIntegratedSecurity("")
                    .ApplyConnectTimeOut("205")
            };
            yield return new object[]
            {
                new ConnectionOptions("something", BackupPath, 5)
                    .ApplyServer("(localDb)")
                    .ApplyDatabaseName("Banana")
                    .ApplyIntegratedSecurity("\t")
                    .ApplyConnectTimeOut("205")
            };
            yield return new object[]
            {
                new ConnectionOptions("something", BackupPath, 5)
                    .ApplyServer("(localDb)")
                    .ApplyDatabaseName("Banana")
                    .ApplyIntegratedSecurity("True")
                    .ApplyConnectTimeOut("-1")
            };
            yield return new object[]
            {
                new ConnectionOptions("something", BackupPath, 5)
                    .ApplyServer("(localDb)")
                    .ApplyDatabaseName("Banana")
                    .ApplyIntegratedSecurity("False")
                    .ApplyConnectTimeOut("-1000")
            };
        }
    }
}
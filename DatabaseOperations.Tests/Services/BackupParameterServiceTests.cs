namespace DatabaseOperations.Tests.Services
{
    using System;
    using System.Data;
    using System.Linq;
    using Constants;
    using DatabaseOperations.DataTransferObjects;
    using DatabaseOperations.Services;
    using FluentAssertions;
    using Interfaces;
using Microsoft.Data.SqlClient;
    using NSubstitute;
    using Options;
    using Xunit;

    public class BackupParameterServiceTests
    {
        public BackupParameterServiceTests()
        {
            DateTimeWrapper.Now.Returns(
                new DateTime(
                    2021,
                    08,
                    11,
                    15,
                    52,
                    08));
        }

        private static readonly IDateTimeWrapper DateTimeWrapper = Substitute.For<IDateTimeWrapper>();

        [Fact]
        public void TestGetPropertiesWithValidDataReturnsDefaultProperties()
        {
            // Arrange.
            OperatorOptions options = new() { BackupPath = @"C:\Backups\", Timeout = 10 };

            // Act.
            BackupProperties actual = BackupParameterService.GetBackupPropertiesForTests(
                GetValidConnectionProperties(),
                options,
                DateTimeWrapper);

            // Assert.
            actual.Should()
                .BeOfType<BackupProperties>()
                .Which.Should()
                .NotBeNull();
        }

        [Fact]
        public void TestGetPropertiesWithValidDataReturnsExpectedFileName()
        {
            // Arrange.
            OperatorOptions options = new() { BackupPath = @"C:\Backups\", Timeout = 10 };

            // Act.
            BackupProperties actual = BackupParameterService.GetBackupPropertiesForTests(
                GetValidConnectionProperties(),
                options,
                DateTimeWrapper);

            // Assert.
            actual.BackupFileName.Should()
                .Be("database_Full_2021-08-11-15-52-08.bak");
        }

        [Fact]
        public void TestGetPropertiesWithValidDataReturnsExpectedDescription()
        {
            // Arrange.
            OperatorOptions options = new() { BackupPath = @"C:\Backups\", Timeout = 10 };

            // Act.
            BackupProperties actual = BackupParameterService.GetBackupPropertiesForTests(
                GetValidConnectionProperties(),
                options,
                DateTimeWrapper);

            // Assert.
            actual.Description.Should()
                .Be("Full backup of the `database` database.");
        }

        [Theory]
        [InlineData(10, 10)]
        [InlineData(1, 1)]
        [InlineData(10000, 10000)]
        [InlineData(0, 3600)]
        public void TestGetPropertiesWithValidDataReturnsTimeout(
            int timeout,
            int expected)
        {
            // Arrange.
            OperatorOptions options = new() { BackupPath = @"C:\Backups\", Timeout = timeout };

            // Act.
            BackupProperties actual = BackupParameterService.GetBackupPropertiesForTests(
                GetValidConnectionProperties(),
                options,
                DateTimeWrapper);

            // Assert.
            actual.CommandTimeout.Should()
                .Be(expected);
        }

        [Fact]
        public void TestGetPropertiesWithValidDataReturnsExpectedBackupPath()
        {
            // Arrange.
            OperatorOptions options = new() { BackupPath = @"C:\Backups\", Timeout = 10 };

            // Act.
            BackupProperties actual = BackupParameterService.GetBackupPropertiesForTests(
                GetValidConnectionProperties(),
                options,
                DateTimeWrapper);

            // Assert.
            actual.BackupPath.Should()
                .Be(options.BackupPath);
        }

        [Fact]
        public void TestGetPropertiesWithValidDataReturnsExpectedBackupParameters()
        {
            // Arrange.
            OperatorOptions options = new() { BackupPath = @"C:\Backups\", Timeout = 10 };

            // Act.
            BackupProperties actual = BackupParameterService.GetBackupPropertiesForTests(
                GetValidConnectionProperties(),
                options,
                DateTimeWrapper);

            // Assert.
            actual.BackupParameters.Should()
                .HaveCount(1);
            actual.BackupParameters[0]
                .SqlDbType.Should()
                .Be(SqlDbType.VarChar);
            actual.BackupParameters[0]
                .Value.Should()
                .Be(options.BackupPath);
        }

        [Fact]
        public void TestGetPropertiesWithValidDataReturnsExpectedExecutionNameParameter()
        {
            // Arrange.
            OperatorOptions options = new() { BackupPath = @"C:\Backups\", Timeout = 10 };

            // Act.
            BackupProperties actual = BackupParameterService.GetBackupPropertiesForTests(
                GetValidConnectionProperties(),
                options,
                DateTimeWrapper);

            SqlParameter[] actualParameters = actual.ExecutionParameters;

            // Assert.
            actualParameters.Should()
                .HaveCount(3);

            string? actualNameValue = actualParameters.First(p => p.ParameterName == Parameters.NameParameter)
                .Value.ToString();
            actualNameValue.Should()
                .NotBeNullOrWhiteSpace();
            actualNameValue.Should()
                .Be("database");
        }

        [Fact]
        public void TestGetPropertiesWithValidDataReturnsExpectedExecutionLocationParameter()
        {
            // Arrange.
            OperatorOptions options = new() { BackupPath = @"C:\Backups\", Timeout = 10 };

            // Act.
            BackupProperties actual = BackupParameterService.GetBackupPropertiesForTests(
                GetValidConnectionProperties(),
                options,
                DateTimeWrapper);

            SqlParameter[] actualParameters = actual.ExecutionParameters;

            // Assert.
            actualParameters.Should()
                .HaveCount(3);

            string? actualLocationValue = actualParameters.First(p => p.ParameterName == Parameters.LocationParameter)
                .Value.ToString();
            actualLocationValue.Should()
                .NotBeNullOrWhiteSpace();
            actualLocationValue.Should()
                .Be(@"C:\Backups\" + "database_Full_2021-08-11-15-52-08.bak");
        }

        [Fact]
        public void TestGetPropertiesWithValidDataReturnsExpectedExecutionDescriptionParameter()
        {
            // Arrange.
            OperatorOptions options = new() { BackupPath = @"C:\Backups\", Timeout = 10 };

            // Act.
            BackupProperties actual = BackupParameterService.GetBackupPropertiesForTests(
                GetValidConnectionProperties(),
                options,
                DateTimeWrapper);

            SqlParameter[] actualParameters = actual.ExecutionParameters;

            // Assert.
            actualParameters.Should()
                .HaveCount(3);

            string? actualDescriptionValue = actualParameters.First(p => p.ParameterName == Parameters.DescriptionParameter)
                .Value.ToString();
            actualDescriptionValue.Should()
                .NotBeNullOrWhiteSpace();
            actualDescriptionValue.Should()
                .Be("Full backup of the `database` database.");
        }

        private static ConnectionProperties GetValidConnectionProperties()
        {
            ConnectionProperties connProps = new()
                { Server = "server", DatabaseName = "database", IntegratedSecurity = "True", ConnectTimeout = "5" };
            return connProps;
        }
    }
}
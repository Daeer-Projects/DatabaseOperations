namespace DatabaseOperations.Tests.Validators
{
    using System.Collections.Generic;
    using DatabaseOperations.DataTransferObjects;
    using DatabaseOperations.Extensions;
    using DatabaseOperations.Validators;
    using FluentAssertions;
    using FluentValidation.Results;
    using Xunit;

    public class ConnectionPropertiesValidatorTests
    {
        [Fact]
        public void TestIsValidWithValidPropertiesReturnsTrue()
        {
            // Arrange.
            ConnectionProperties properties = new()
                { Server = "127.0.0.1", DatabaseName = "Bananas", ConnectTimeout = "5", UserId = "1", Password = "password" };

            // Act.
            ValidationResult validationResult = properties.CheckValidation(new ConnectionPropertiesValidator());

            // Assert.
            validationResult.IsValid.Should()
                .BeTrue();
        }

        [Theory]
        [InlineData("SSPI")]
        [InlineData("True")]
        [InlineData("False")]
        public void TestIsValidWithSecuritySetAndMissingUserReturnsTrue(string security)
        {
            // Arrange.
            ConnectionProperties properties = new()
                { Server = "127.0.0.1", DatabaseName = "Bananas", ConnectTimeout = "5", IntegratedSecurity = security };

            // Act.
            ValidationResult validationResult = properties.CheckValidation(new ConnectionPropertiesValidator());

            // Assert.
            validationResult.IsValid.Should()
                .BeTrue();
        }

        [Fact]
        public void TestIsValidWithMissingServerReturnsFalse()
        {
            // Arrange.
            ConnectionProperties properties = new()
                { DatabaseName = "Bananas", ConnectTimeout = "5", IntegratedSecurity = "SSPI" };

            // Act.
            ValidationResult validationResult = properties.CheckValidation(new ConnectionPropertiesValidator());

            // Assert.
            validationResult.IsValid.Should()
                .BeFalse();
        }

        [Fact]
        public void TestIsValidWithMissingDatabaseReturnsFalse()
        {
            // Arrange.
            ConnectionProperties properties = new()
                { Server = "127.0.0.1", ConnectTimeout = "5", IntegratedSecurity = "SSPI" };

            // Act.
            ValidationResult validationResult = properties.CheckValidation(new ConnectionPropertiesValidator());

            // Assert.
            validationResult.IsValid.Should()
                .BeFalse();
        }

        [Fact]
        public void TestIsValidWithMissingUserAndSecurityReturnsFalse()
        {
            // Arrange.
            ConnectionProperties properties = new()
                { Server = "127.0.0.1", DatabaseName = "Bananas", ConnectTimeout = "5" };

            // Act.
            ValidationResult validationResult = properties.CheckValidation(new ConnectionPropertiesValidator());

            // Assert.
            validationResult.IsValid.Should()
                .BeFalse();
        }

        [Theory]
        [MemberData(nameof(ConnectionProperties))]
        public void TestIsValidWithInvalidPropertiesReturnsFalse(ConnectionProperties properties)
        {
            // Arrange.
            // Act.
            ValidationResult validationResult = properties.CheckValidation(new ConnectionPropertiesValidator());

            // Assert.
            validationResult.IsValid.Should()
                .BeFalse();
        }

        internal static IEnumerable<object[]> ConnectionProperties()
        {
            yield return new object[]
            {
                new ConnectionProperties
                {
                    DatabaseName = "Bananas", ConnectTimeout = "5", IntegratedSecurity = "SSPI"
                }
            };
            yield return new object[]
            {
                new ConnectionProperties
                {
                    Server = "   ", DatabaseName = "Bananas", ConnectTimeout = "5", IntegratedSecurity = "SSPI"
                }
            };
            yield return new object[]
            {
                new ConnectionProperties
                {
                    Server = "127.0.0.1", ConnectTimeout = "5", IntegratedSecurity = "SSPI"
                }
            };
            yield return new object[]
            {
                new ConnectionProperties
                {
                    Server = "127.0.0.1", DatabaseName = "  ", ConnectTimeout = "5", IntegratedSecurity = "SSPI"
                }
            };
            yield return new object[]
            {
                new ConnectionProperties
                {
                    Server = "127.0.0.1", DatabaseName = "Bananas", ConnectTimeout = "5"
                }
            };
            yield return new object[]
            {
                new ConnectionProperties
                {
                    Server = "127.0.0.1", DatabaseName = "Bananas", ConnectTimeout = "-1", IntegratedSecurity = "SSPI"
                }
            };
            yield return new object[]
            {
                new ConnectionProperties
                {
                    Server = "127.0.0.1", DatabaseName = "Bananas", ConnectTimeout = "5", IntegratedSecurity = "hello"
                }
            };
        }
    }
}
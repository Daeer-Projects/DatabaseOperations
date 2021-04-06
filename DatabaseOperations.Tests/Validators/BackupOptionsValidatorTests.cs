using DatabaseOperations.DataTransferObjects;
using DatabaseOperations.Extensions;
using DatabaseOperations.Validators;
using FluentAssertions;
using FluentValidation.Results;
using Xunit;

namespace DatabaseOperations.Tests.Validators
{
    public class BackupOptionsValidatorTests
	{
		[Fact]
		public void TestBackupValidatorWithValidOptionsReturnsTrue()
		{
			// Arrange.
			BackupOptions options = new BackupOptions
			{
				ConnectionString = "server=127.0.0.1; database=bananas;",
				DatabaseName = "Bananas",
				Destination = "G:\\Here\\"
			};

			// Act.
			ValidationResult results = options.CheckValidation<BackupOptions>(new BackupOptionsValidator());

			// Assert.
			results.IsValid.Should().BeTrue();
		}

		[Fact]
		public void TestBackupValidatorWithNullConnectionStringReturnsFalse()
		{
			// Arrange.
			BackupOptions options = new BackupOptions
			{
				DatabaseName = "Bananas",
				Destination = "G:\\Here\\"
			};

			// Act.
			ValidationResult results = options.CheckValidation<BackupOptions>(new BackupOptionsValidator());

			// Assert.
			results.IsValid.Should().BeFalse();
		}

		[Fact]
		public void TestBackupValidatorWithNullDatabaseNameReturnsFalse()
		{
			// Arrange.
			BackupOptions options = new BackupOptions
			{
				ConnectionString = "server=127.0.0.1; database=bananas;",
				Destination = "G:\\Here\\"
			};

			// Act.
			ValidationResult results = options.CheckValidation<BackupOptions>(new BackupOptionsValidator());

			// Assert.
			results.IsValid.Should().BeFalse();
		}

		[Fact]
		public void TestBackupValidatorWithNullDestinationReturnsFalse()
		{
			// Arrange.
			BackupOptions options = new BackupOptions
			{
				ConnectionString = "server=127.0.0.1; database=bananas;",
				DatabaseName = "Bananas"
			};

			// Act.
			ValidationResult results = options.CheckValidation<BackupOptions>(new BackupOptionsValidator());

			// Assert.
			results.IsValid.Should().BeFalse();
		}

		[Fact]
		public void TestBackupValidatorWithEmptyConnectionStringReturnsFalse()
		{
			// Arrange.
			BackupOptions options = new BackupOptions
			{
				ConnectionString = string.Empty,
				DatabaseName = "Bananas",
				Destination = "G:\\Here\\"
			};

			// Act.
			ValidationResult results = options.CheckValidation<BackupOptions>(new BackupOptionsValidator());

			// Assert.
			results.IsValid.Should().BeFalse();
		}

		[Fact]
		public void TestBackupValidatorWithEmptyDatabaseNameReturnsFalse()
		{
			// Arrange.
			BackupOptions options = new BackupOptions
			{
				ConnectionString = "server=127.0.0.1; database=bananas;",
				DatabaseName = string.Empty,
				Destination = "G:\\Here\\"
			};

			// Act.
			ValidationResult results = options.CheckValidation<BackupOptions>(new BackupOptionsValidator());

			// Assert.
			results.IsValid.Should().BeFalse();
		}

		[Fact]
		public void TestBackupValidatorWithEmptyDestinationReturnsFalse()
		{
			// Arrange.
			BackupOptions options = new BackupOptions
			{
				ConnectionString = "server=127.0.0.1; database=bananas;",
				DatabaseName = "Bananas",
				Destination = string.Empty
			};

			// Act.
			ValidationResult results = options.CheckValidation<BackupOptions>(new BackupOptionsValidator());

			// Assert.
			results.IsValid.Should().BeFalse();
		}
	}
}

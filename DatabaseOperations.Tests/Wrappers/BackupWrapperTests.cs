using DatabaseOperations.Interfaces;
using DatabaseOperations.Wrappers;
using FluentAssertions;
using Microsoft.SqlServer.Management.Smo;
using Xunit;

namespace DatabaseOperations.Tests.Wrappers
{
    public class BackupWrapperTests
	{
		private readonly IBackupWrapper _backupWrapper = new BackupWrapper();

		[Fact]
		public void TestActionTypeWithActionTypeReturnsExpected()
		{
			// Arrange.
			const BackupActionType expected = BackupActionType.Files;
			_backupWrapper.ActionType = expected;

			// Act.
			var actual = _backupWrapper.ActionType;

			// Assert.
			actual.Should().Be(expected);
		}

		[Fact]
		public void TestBackupSetDescriptionWithDescriptionReturnsExpected()
		{
			// Arrange.
			const string expected = "A good backup.";
			_backupWrapper.BackupSetDescription = expected;

			// Act.
			var actual = _backupWrapper.BackupSetDescription;

			// Assert.
			actual.Should().Be(expected);
		}

		[Fact]
		public void TestDatabaseWithDatabaseReturnsExpected()
		{
			// Arrange.
			const string expected = "A good database.";
			_backupWrapper.Database = expected;

			// Act.
			var actual = _backupWrapper.Database;

			// Assert.
			actual.Should().Be(expected);
		}

		[Fact]
		public void TestBackupSetNameWithSetNameReturnsExpected()
		{
			// Arrange.
			const string expected = "A good backup set name.";
			_backupWrapper.BackupSetName = expected;

			// Act.
			var actual = _backupWrapper.BackupSetName;

			// Assert.
			actual.Should().Be(expected);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void TestInitialiseWithDefinedValueReturnsExpected(bool expected)
		{
			// Arrange.
			_backupWrapper.Initialize = expected;

			// Act.
			var actual = _backupWrapper.Initialize;

			// Assert.
			actual.Should().Be(expected);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void TestChecksumWithDefinedValueReturnsExpected(bool expected)
		{
			// Arrange.
			_backupWrapper.Checksum = expected;

			// Act.
			var actual = _backupWrapper.Checksum;

			// Assert.
			actual.Should().Be(expected);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void TestContinueAfterErrorWithDefinedValueReturnsExpected(bool expected)
		{
			// Arrange.
			_backupWrapper.ContinueAfterError = expected;

			// Act.
			var actual = _backupWrapper.ContinueAfterError;

			// Assert.
			actual.Should().Be(expected);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void TestIncrementalWithDefinedValueReturnsExpected(bool expected)
		{
			// Arrange.
			_backupWrapper.Incremental = expected;

			// Act.
			var actual = _backupWrapper.Incremental;

			// Assert.
			actual.Should().Be(expected);
		}

		[Theory]
		[InlineData(BackupTruncateLogType.NoTruncate)]
		[InlineData(BackupTruncateLogType.Truncate)]
		[InlineData(BackupTruncateLogType.TruncateOnly)]
		public void TestBackupTruncateLogTypeWithDefinedValueReturnsExpected(BackupTruncateLogType expected)
		{
			// Arrange.
			_backupWrapper.LogTruncation = expected;

			// Act.
			var actual = _backupWrapper.LogTruncation;

			// Assert.
			actual.Should().Be(expected);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void TestFormatMediaWithDefinedValueReturnsExpected(bool expected)
		{
			// Arrange.
			_backupWrapper.FormatMedia = expected;

			// Act.
			var actual = _backupWrapper.FormatMedia;

			// Assert.
			actual.Should().Be(expected);
		}

		[Fact]
		public void TestAddDeviceWithDeviceAddsNewDevice()
		{
			// Arrange.
			var deviceItem =
				new BackupDeviceItem(
					"TestBackupDevice.bak",
					DeviceType.File);
			_backupWrapper.AddDevice(deviceItem);


			// Act.
			var actual = _backupWrapper.GetDeviceList();

			// Assert.
			actual.Contains(deviceItem).Should().BeTrue();
		}

		[Fact]
		public void TestRemoveDeviceWithDeviceRemovesDevice()
		{
			// Arrange.
			var deviceItem =
				new BackupDeviceItem(
					"TestBackupDevice.bak",
					DeviceType.File);
			_backupWrapper.AddDevice(deviceItem);
			var currentList = _backupWrapper.GetDeviceList();
			currentList.Count.Should().Be(1);
			_backupWrapper.RemoveDevice(deviceItem);

			// Act.
			var actual = _backupWrapper.GetDeviceList();

			// Assert.
			actual.Contains(deviceItem).Should().BeFalse();
			actual.Count.Should().Be(0);
		}
	}
}

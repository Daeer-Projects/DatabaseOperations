using System;
using System.Collections.Generic;
using DatabaseOperations.Extensions;
using DatabaseOperations.Tests.TestTypes;
using FluentAssertions;
using Xunit;

namespace DatabaseOperations.Tests.Extensions
{
    public class ValidationExtensionsTests
	{
		[Theory]
		[MemberData(nameof(GetGoodTestType))]
		public void TestCheckValidationWithValidTestTypeReturnsTrue(TestType testType)
		{
			// Arrange.
			// Act.
			var result = testType.CheckValidation(new TestValidator());

			// Assert.
			result.IsValid.Should().BeTrue("the testType data is all made up of valid data.");
		}

		[Theory]
		[MemberData(nameof(GetBadTestType))]
		public void TestCheckValidationWithInvalidTestTypeReturnsFalse(TestType testType)
		{
			// Arrange.
			// Act.
			var result = testType.CheckValidation(new TestValidator());

			// Assert.
			result.IsValid.Should().BeFalse("the testType data is made up of invalid data.");
		}

		#region Test Data.

		public static IEnumerable<object[]> GetGoodTestType
		{
			get
			{
				yield return new object[]
				{
					new TestType
					{
						TestIdentity = 1,
						TestName = "TestOne",
						TestDateTime = new DateTime(2019, 01, 01)
					}
				};
				yield return new object[]
				{
					new TestType
					{
						TestIdentity = 2,
						TestName = "TestTwo",
						TestDateTime = new DateTime(2019, 01, 02)
					}
				};
				yield return new object[]
				{
					new TestType
					{
						TestIdentity = 2,
						TestName = "TestThree",
						TestDateTime = new DateTime(2019, 01, 03)
					}
				};
			}
		}

		public static IEnumerable<object[]> GetBadTestType
		{
			get
			{
				yield return new object[]
				{
					new TestType
					{
						TestIdentity = -100,
						TestName = "TestOne",
						TestDateTime = new DateTime(2019, 01, 01)
					}
				};
				yield return new object[]
				{
					new TestType
					{
						TestIdentity = -1,
						TestName = "TestOne",
						TestDateTime = new DateTime(2019, 01, 01)
					}
				};
				yield return new object[]
				{
					new TestType
					{
						TestIdentity = 0,
						TestName = "TestOne",
						TestDateTime = new DateTime(2019, 01, 01)
					}
				};
				yield return new object[]
				{
					new TestType
					{
						TestIdentity = 1,
						TestName = string.Empty,
						TestDateTime = new DateTime(2019, 01, 02)
					}
				};
				yield return new object[]
				{
					new TestType
					{
						TestIdentity = 2,
						TestName = null,
						TestDateTime = new DateTime(2019, 01, 02)
					}
				};
				yield return new object[]
				{
					new TestType
					{
						TestIdentity = 2,
						TestName = "    ",
						TestDateTime = new DateTime(2019, 01, 02)
					}
				};
				yield return new object[]
				{
					new TestType
					{
						TestIdentity = 3,
						TestName = "TestThree",
						TestDateTime = new DateTime(2009, 12, 31)
					}
				};
				yield return new object[]
				{
					new TestType
					{
						TestIdentity = 100,
						TestName = "TestThree",
						TestDateTime = new DateTime(2031, 01, 01)
					}
				};
			}
		}

		#endregion Test Data.
	}
}

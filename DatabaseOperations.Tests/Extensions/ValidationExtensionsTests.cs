using System;
using System.Collections.Generic;
using DatabaseOperations.Extensions;
using DatabaseOperations.Tests.TestTypes;
using FluentAssertions;
using Xunit;
using Type = DatabaseOperations.Tests.TestTypes.Type;

namespace DatabaseOperations.Tests.Extensions
{
    public class ValidationExtensionsTests
	{
		[Theory]
		[MemberData(nameof(GetGoodTestType))]
		public void TestCheckValidationWithValidTestTypeReturnsTrue(Type testType)
		{
			// Arrange.
			// Act.
			var result = testType.CheckValidation(new TestValidator());

			// Assert.
			result.IsValid.Should().BeTrue("the testType data is all made up of valid data.");
		}

		[Theory]
		[MemberData(nameof(GetBadTestType))]
		public void TestCheckValidationWithInvalidTestTypeReturnsFalse(Type testType)
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
					new Type
					{
						Identity = 1,
						Name = "TestOne",
						DateTime = new DateTime(2019, 01, 01)
					}
				};
				yield return new object[]
				{
					new Type
					{
						Identity = 2,
						Name = "TestTwo",
						DateTime = new DateTime(2019, 01, 02)
					}
				};
				yield return new object[]
				{
					new Type
					{
						Identity = 2,
						Name = "TestThree",
						DateTime = new DateTime(2019, 01, 03)
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
					new Type
					{
						Identity = -100,
						Name = "TestOne",
						DateTime = new DateTime(2019, 01, 01)
					}
				};
				yield return new object[]
				{
					new Type
					{
						Identity = -1,
						Name = "TestOne",
						DateTime = new DateTime(2019, 01, 01)
					}
				};
				yield return new object[]
				{
					new Type
					{
						Identity = 0,
						Name = "TestOne",
						DateTime = new DateTime(2019, 01, 01)
					}
				};
				yield return new object[]
				{
					new Type
					{
						Identity = 1,
						Name = string.Empty,
						DateTime = new DateTime(2019, 01, 02)
					}
				};
				yield return new object[]
				{
					new Type
					{
						Identity = 2,
						Name = null!,
						DateTime = new DateTime(2019, 01, 02)
					}
				};
				yield return new object[]
				{
					new Type
					{
						Identity = 2,
						Name = "    ",
						DateTime = new DateTime(2019, 01, 02)
					}
				};
				yield return new object[]
				{
					new Type
					{
						Identity = 3,
						Name = "TestThree",
						DateTime = new DateTime(2009, 12, 31)
					}
				};
				yield return new object[]
				{
					new Type
					{
						Identity = 100,
						Name = "TestThree",
						DateTime = new DateTime(2031, 01, 01)
					}
				};
			}
		}

		#endregion Test Data.
	}
}

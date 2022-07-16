namespace DatabaseOperations.Tests.TestTypes
{
    using System;
    using FluentValidation;

    internal class TestValidator : AbstractValidator<Type>
    {
        public TestValidator()
        {
            RuleFor(test => test)
                .NotNull();
            RuleFor(test => test.Identity)
                .GreaterThan(0);
            RuleFor(test => test.Name)
                .Must(TestNameMustNotBeNullOrWhiteSpace);
            RuleFor(test => test.DateTime)
                .InclusiveBetween(new DateTime(2010, 01, 01), new DateTime(2030, 12, 31));
        }

        private static bool TestNameMustNotBeNullOrWhiteSpace(string testName)
        {
            return !string.IsNullOrWhiteSpace(testName);
        }
    }
}
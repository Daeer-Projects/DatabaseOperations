namespace DatabaseOperations.Extensions
{
    using FluentValidation;
    using FluentValidation.Results;

    internal static class ValidationExtensions
    {
        internal static ValidationResult CheckValidation<T>(
            this T component,
            AbstractValidator<T> validator)
        {
            ValidationResult? result = validator.Validate(component);
            return result;
        }
    }
}
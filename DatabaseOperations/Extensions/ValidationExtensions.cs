using FluentValidation;
using FluentValidation.Results;

namespace DatabaseOperations.Extensions
{
    internal static class ValidationExtensions
    {
        internal static ValidationResult CheckValidation<T>(this T component, AbstractValidator<T> validator) where T : class
        {
            var result = validator.Validate(component);
            return result;
        }
    }
}

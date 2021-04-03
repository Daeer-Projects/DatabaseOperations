using FluentValidation;
using FluentValidation.Results;

namespace DatabaseOperations.Core.Extensions
{
    public static class ValidationExtensions
	{
        public static ValidationResult CheckValidation<T>(this T component, AbstractValidator<T> validator) where T : class
        {
            ValidationResult result = validator.Validate(component);
            return result;
        }
	}
}

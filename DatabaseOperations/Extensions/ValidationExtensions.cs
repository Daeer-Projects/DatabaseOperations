using FluentValidation;
using FluentValidation.Results;

namespace DatabaseOperations.Extensions
{
    public static class ValidationExtensions
	{
        public static ValidationResult CheckValidation<T>(this T component, AbstractValidator<T> validator) where T : class
        {
            var result = validator.Validate(component);
            return result;
        }
	}
}

using DatabaseOperations.DataTransferObjects;
using FluentValidation;

namespace DatabaseOperations.Validators
{
    internal class ConnectionOptionsValidator : AbstractValidator<ConnectionOptions>
    {
        internal ConnectionOptionsValidator()
        {
            RuleFor(options => options).NotNull();
            RuleFor(options => options.Server).NotNull().NotEmpty();
            RuleFor(options => options.DatabaseName).NotNull().NotEmpty();
            RuleFor(options => options.ConnectTimeout).Must(ConnectTimeoutCanNotBeNegative);
            RuleFor(options => options).Must(UserAndPasswordMustBeSuppliedIfSecurityNotSet);
            RuleFor(options => options.IntegratedSecurity).Must(SecurityIfSetMustBeOneOfThreeOptions);
        }

        private static bool ConnectTimeoutCanNotBeNegative(string timeout)
        {
            var result = int.TryParse(timeout, out var convertedTimeout);
            if (result) result = convertedTimeout >= 0;
            
            return result;
        }

        private static bool UserAndPasswordMustBeSuppliedIfSecurityNotSet(ConnectionOptions options)
        {
            if (!string.IsNullOrWhiteSpace(options.IntegratedSecurity)) return true;
            var userSet = !string.IsNullOrWhiteSpace(options.UserId);
            var passwordSet = !string.IsNullOrWhiteSpace(options.Password);

            var result = userSet && passwordSet;

            return result;
        }

        private static bool SecurityIfSetMustBeOneOfThreeOptions(string integratedSecurity)
        {
            if (string.IsNullOrWhiteSpace(integratedSecurity)) return true;

            var result = integratedSecurity.ToLower() switch
            {
                "true" => true,
                "false" => true,
                "sspi" => true,
                _ => false
            };

            return result;
        }
    }
}
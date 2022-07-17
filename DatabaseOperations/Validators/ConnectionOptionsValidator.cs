namespace DatabaseOperations.Validators
{
    using DataTransferObjects;
    using FluentValidation;

    internal class ConnectionOptionsValidator : AbstractValidator<ConnectionOptions>
    {
        internal ConnectionOptionsValidator()
        {
            RuleFor(options => options)
                .NotNull();
            RuleFor(options => options.Server)
                .NotNull()
                .NotEmpty();
            RuleFor(options => options.DatabaseName)
                .NotNull()
                .NotEmpty();
            RuleFor(options => options.ConnectTimeout)
                .Must(ConnectTimeoutCanNotBeNegative);
            RuleFor(options => options)
                .Must(UserAndPasswordMustBeSuppliedIfSecurityNotSet);
            RuleFor(options => options.IntegratedSecurity)
                .Must(SecurityIfSetMustBeOneOfThreeOptions);
        }

        private static bool ConnectTimeoutCanNotBeNegative(string timeout)
        {
            bool result = int.TryParse(timeout, out int convertedTimeout);
            if (result) result = convertedTimeout >= 0;

            return result;
        }

        private static bool UserAndPasswordMustBeSuppliedIfSecurityNotSet(ConnectionOptions options)
        {
            if (!string.IsNullOrWhiteSpace(options.IntegratedSecurity)) return true;
            bool userSet = !string.IsNullOrWhiteSpace(options.UserId);
            bool passwordSet = !string.IsNullOrWhiteSpace(options.Password);

            bool result = userSet && passwordSet;

            return result;
        }

        private static bool SecurityIfSetMustBeOneOfThreeOptions(string integratedSecurity)
        {
            if (string.IsNullOrWhiteSpace(integratedSecurity)) return true;

            bool result = integratedSecurity.ToLower() switch
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
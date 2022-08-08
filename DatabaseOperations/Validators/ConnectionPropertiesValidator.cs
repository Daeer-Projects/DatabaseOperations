namespace DatabaseOperations.Validators
{
    using DataTransferObjects;
    using FluentValidation;

    internal class ConnectionPropertiesValidator : AbstractValidator<ConnectionProperties>
    {
        public ConnectionPropertiesValidator()
        {
            RuleFor(properties => properties)
                .NotNull();
            RuleFor(properties => properties.Server)
                .NotNull()
                .NotEmpty();
            RuleFor(properties => properties.DatabaseName)
                .NotNull()
                .NotEmpty();
            RuleFor(properties => properties.ConnectTimeout)
                .Must(ConnectTimeoutCanNotBeNegative);
            RuleFor(properties => properties)
                .Must(UserAndPasswordMustBeSuppliedIfSecurityNotSet);
            RuleFor(properties => properties.IntegratedSecurity)
                .Must(SecurityIfSetMustBeOneOfThreeOptions);
        }

        private static bool ConnectTimeoutCanNotBeNegative(string timeout)
        {
            bool result = int.TryParse(timeout, out int convertedTimeout);
            if (result) result = convertedTimeout >= 0;

            return result;
        }

        private static bool UserAndPasswordMustBeSuppliedIfSecurityNotSet(ConnectionProperties properties)
        {
            if (!string.IsNullOrWhiteSpace(properties.IntegratedSecurity)) return true;
            bool userSet = !string.IsNullOrWhiteSpace(properties.UserId);
            bool passwordSet = !string.IsNullOrWhiteSpace(properties.Password);

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

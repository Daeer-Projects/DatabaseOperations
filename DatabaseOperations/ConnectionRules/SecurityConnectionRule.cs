namespace DatabaseOperations.ConnectionRules
{
    using DataTransferObjects;
    using Extensions;
    using Interfaces;

    internal class SecurityConnectionRule : IConnectionRule
    {
        private const string IntegratedSecurityLookUp = "integrated security";

        public bool Check(string item)
        {
            return item.ToLower()
                .Contains(IntegratedSecurityLookUp);
        }

        public ConnectionOptions ApplyChange(
            ConnectionOptions options,
            string item)
        {
            if (string.IsNullOrWhiteSpace(options.IntegratedSecurity))
                options.IntegratedSecurity = IntegratedSecurityLookUp.ToValue(item);
            return options;
        }

        public ConnectionProperties ApplyChange(
            ConnectionProperties properties,
            string item)
        {
            if (string.IsNullOrWhiteSpace(properties.IntegratedSecurity))
                properties.IntegratedSecurity = IntegratedSecurityLookUp.ToValue(item);
            return properties;
        }
    }
}
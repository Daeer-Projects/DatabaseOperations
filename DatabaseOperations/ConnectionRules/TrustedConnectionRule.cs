namespace DatabaseOperations.ConnectionRules
{
    using DataTransferObjects;
    using Extensions;
    using Interfaces;

    internal class TrustedConnectionRule : IConnectionRule
    {
        private const string TrustedConnectionLookUp = "trusted_connection";

        public bool Check(string item)
        {
            return item.ToLower()
                .Contains(TrustedConnectionLookUp);
        }

        public ConnectionOptions ApplyChange(
            ConnectionOptions options,
            string item)
        {
            if (string.IsNullOrWhiteSpace(options.IntegratedSecurity))
                options.IntegratedSecurity = TrustedConnectionLookUp.ToValue(item);
            return options;
        }
    }
}
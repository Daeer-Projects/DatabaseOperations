namespace DatabaseOperations.ConnectionRules
{
    using DataTransferObjects;
    using Extensions;
    using Interfaces;

    internal class ConnectTimeoutConnectionRule : IConnectionRule
    {
        private const string ConnectTimeoutLookUp = "connect timeout";

        public bool Check(string item)
        {
            return item.ToLower()
                .Contains(ConnectTimeoutLookUp);
        }

        public ConnectionOptions ApplyChange(
            ConnectionOptions options,
            string item)
        {
            if (string.IsNullOrWhiteSpace(options.ConnectTimeout)) options.ConnectTimeout = ConnectTimeoutLookUp.ToValue(item);
            return options;
        }
    }
}
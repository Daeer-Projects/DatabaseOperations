namespace DatabaseOperations.ConnectionRules
{
    using DataTransferObjects;
    using Extensions;
    using Interfaces;

    internal class ConnectionTimeoutConnectionRule : IConnectionRule
    {
        private const string ConnectionTimeoutLookUp = "connection timeout";

        public bool Check(string item)
        {
            return item.ToLower()
                .Contains(ConnectionTimeoutLookUp);
        }

        public ConnectionOptions ApplyChange(
            ConnectionOptions options,
            string item)
        {
            if (string.IsNullOrWhiteSpace(options.ConnectTimeout)) options.ConnectTimeout = ConnectionTimeoutLookUp.ToValue(item);
            return options;
        }
    }
}
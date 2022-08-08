namespace DatabaseOperations.ConnectionRules
{
    using DataTransferObjects;
    using Extensions;
    using Interfaces;

    internal class ServerConnectionRule : IConnectionRule
    {
        private const string ServerLookUp = "server";

        public bool Check(string item)
        {
            return item.ToLower()
                .Contains(ServerLookUp);
        }

        public ConnectionOptions ApplyChange(
            ConnectionOptions options,
            string item)
        {
            if (string.IsNullOrWhiteSpace(options.Server)) options.Server = ServerLookUp.ToValue(item);
            return options;
        }

        public ConnectionProperties ApplyChange(
            ConnectionProperties properties,
            string item)
        {
            if (string.IsNullOrWhiteSpace(properties.Server)) properties.Server = ServerLookUp.ToValue(item);
            return properties;
        }
    }
}
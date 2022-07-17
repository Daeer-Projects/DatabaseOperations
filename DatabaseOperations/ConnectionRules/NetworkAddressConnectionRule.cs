namespace DatabaseOperations.ConnectionRules
{
    using DataTransferObjects;
    using Extensions;
    using Interfaces;

    internal class NetworkAddressConnectionRule : IConnectionRule
    {
        private const string NetworkAddressLookUp = "network address";

        public bool Check(string item)
        {
            return item.ToLower()
                .Contains(NetworkAddressLookUp);
        }

        public ConnectionOptions ApplyChange(
            ConnectionOptions options,
            string item)
        {
            if (string.IsNullOrWhiteSpace(options.Server)) options.Server = NetworkAddressLookUp.ToValue(item);
            return options;
        }
    }
}
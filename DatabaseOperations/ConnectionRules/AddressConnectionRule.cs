namespace DatabaseOperations.ConnectionRules
{
    using DataTransferObjects;
    using Extensions;
    using Interfaces;

    internal class AddressConnectionRule : IConnectionRule
    {
        private const string AddressLookUp = "address";

        public bool Check(string item)
        {
            return item.ToLower()
                .Contains(AddressLookUp);
        }

        public ConnectionOptions ApplyChange(
            ConnectionOptions options,
            string item)
        {
            if (string.IsNullOrWhiteSpace(options.Server)) options.Server = AddressLookUp.ToValue(item);
            return options;
        }

        public ConnectionProperties ApplyChange(
            ConnectionProperties properties,
            string item)
        {
            if (string.IsNullOrWhiteSpace(properties.Server)) properties.Server = AddressLookUp.ToValue(item);
            return properties;
        }
    }
}
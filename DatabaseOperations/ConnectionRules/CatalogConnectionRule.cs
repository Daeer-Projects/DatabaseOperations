namespace DatabaseOperations.ConnectionRules
{
    using DataTransferObjects;
    using Extensions;
    using Interfaces;

    internal class CatalogConnectionRule : IConnectionRule
    {
        private const string InitialCatalogLookUp = "initial catalog";

        public bool Check(string item)
        {
            return item.ToLower()
                .Contains(InitialCatalogLookUp);
        }

        public ConnectionOptions ApplyChange(
            ConnectionOptions options,
            string item)
        {
            if (string.IsNullOrWhiteSpace(options.DatabaseName)) options.DatabaseName = InitialCatalogLookUp.ToValue(item);
            return options;
        }

        public ConnectionProperties ApplyChange(
            ConnectionProperties properties,
            string item)
        {
            if (string.IsNullOrWhiteSpace(properties.DatabaseName)) properties.DatabaseName = InitialCatalogLookUp.ToValue(item);
            return properties;
        }
    }
}
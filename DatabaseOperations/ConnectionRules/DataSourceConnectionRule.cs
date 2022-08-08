namespace DatabaseOperations.ConnectionRules
{
    using DataTransferObjects;
    using Extensions;
    using Interfaces;

    internal class DataSourceConnectionRule : IConnectionRule
    {
        private const string DataSourceLookUp = "data source";

        public bool Check(string item)
        {
            return item.ToLower()
                .Contains(DataSourceLookUp);
        }

        public ConnectionOptions ApplyChange(
            ConnectionOptions options,
            string item)
        {
            if (string.IsNullOrWhiteSpace(options.Server)) options.Server = DataSourceLookUp.ToValue(item);
            return options;
        }

        public ConnectionProperties ApplyChange(
            ConnectionProperties properties,
            string item)
        {
            if (string.IsNullOrWhiteSpace(properties.Server)) properties.Server = DataSourceLookUp.ToValue(item);
            return properties;
        }
    }
}
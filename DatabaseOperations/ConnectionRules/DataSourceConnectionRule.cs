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
    }
}
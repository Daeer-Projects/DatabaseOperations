namespace DatabaseOperations.ConnectionRules
{
    using DataTransferObjects;
    using Extensions;
    using Interfaces;

    internal class DatabaseConnectionRule : IConnectionRule
    {
        private const string DatabaseLookUp = "database";

        public bool Check(string item)
        {
            return item.ToLower()
                .Contains(DatabaseLookUp);
        }

        public ConnectionOptions ApplyChange(
            ConnectionOptions options,
            string item)
        {
            if (string.IsNullOrWhiteSpace(options.DatabaseName)) options.DatabaseName = DatabaseLookUp.ToValue(item);
            return options;
        }
    }
}
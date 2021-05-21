using DatabaseOperations.DataTransferObjects;
using DatabaseOperations.Extensions;
using DatabaseOperations.Interfaces;

namespace DatabaseOperations.ConnectionRules
{
    internal class DatabaseConnectionRule : IConnectionRule
    {
        private const string DatabaseLookUp = "database";

        public bool Check(string item)
        {
            return item.ToLower().Contains(DatabaseLookUp);
        }

        public ConnectionOptions ApplyChange(ConnectionOptions options, string item)
        {
            if(string.IsNullOrWhiteSpace(options.DatabaseName)) options.DatabaseName = DatabaseLookUp.ToValue(item);
            return options;
        }
    }
}
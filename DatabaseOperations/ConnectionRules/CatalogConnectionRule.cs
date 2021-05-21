using DatabaseOperations.DataTransferObjects;
using DatabaseOperations.Extensions;
using DatabaseOperations.Interfaces;

namespace DatabaseOperations.ConnectionRules
{
    internal class CatalogConnectionRule : IConnectionRule
    {
        private const string InitialCatalogLookUp = "initial catalog";

        public bool Check(string item)
        {
            return item.ToLower().Contains(InitialCatalogLookUp);
        }

        public ConnectionOptions ApplyChange(ConnectionOptions options, string item)
        {
            if(string.IsNullOrWhiteSpace(options.DatabaseName)) options.DatabaseName = InitialCatalogLookUp.ToValue(item);
            return options;
        }
    }
}
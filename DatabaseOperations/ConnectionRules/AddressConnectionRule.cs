using DatabaseOperations.DataTransferObjects;
using DatabaseOperations.Extensions;
using DatabaseOperations.Interfaces;

namespace DatabaseOperations.ConnectionRules
{
    internal class AddressConnectionRule : IConnectionRule
    {
        private const string AddressLookUp = "address";

        public bool Check(string item)
        {
            return item.ToLower().Contains(AddressLookUp);
        }

        public ConnectionOptions ApplyChange(ConnectionOptions options, string item)
        {
            if(string.IsNullOrWhiteSpace(options.Server)) options.Server = AddressLookUp.ToValue(item);
            return options;
        }
    }
}
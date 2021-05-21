using DatabaseOperations.DataTransferObjects;
using DatabaseOperations.Extensions;
using DatabaseOperations.Interfaces;

namespace DatabaseOperations.ConnectionRules
{
    internal class AbbreviatedAddressConnectionRule : IConnectionRule
    {
        private const string AbbreviatedAddressLookUp = "addr";

        public bool Check(string item)
        {
            return item.ToLower().Contains(AbbreviatedAddressLookUp);
        }

        public ConnectionOptions ApplyChange(ConnectionOptions options, string item)
        {
            if(string.IsNullOrWhiteSpace(options.Server)) options.Server = AbbreviatedAddressLookUp.ToValue(item);
            return options;
        }
    }
}
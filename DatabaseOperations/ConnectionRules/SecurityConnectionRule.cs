using DatabaseOperations.DataTransferObjects;
using DatabaseOperations.Extensions;
using DatabaseOperations.Interfaces;

namespace DatabaseOperations.ConnectionRules
{
    internal class SecurityConnectionRule : IConnectionRule
    {
        private const string IntegratedSecurityLookUp = "integrated security";

        public bool Check(string item)
        {
            return item.ToLower().Contains(IntegratedSecurityLookUp);
        }

        public ConnectionOptions ApplyChange(ConnectionOptions options, string item)
        {
            if(string.IsNullOrWhiteSpace(options.IntegratedSecurity)) options.IntegratedSecurity = IntegratedSecurityLookUp.ToValue(item);
            return options;
        }
    }
}
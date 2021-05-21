using DatabaseOperations.DataTransferObjects;
using DatabaseOperations.Extensions;
using DatabaseOperations.Interfaces;

namespace DatabaseOperations.ConnectionRules
{
    internal class PasswordConnectionRule : IConnectionRule
    {
        private const string PasswordLookUp = "password";

        public bool Check(string item)
        {
            return item.ToLower().Contains(PasswordLookUp);
        }

        public ConnectionOptions ApplyChange(ConnectionOptions options, string item)
        {
            if(string.IsNullOrWhiteSpace(options.Password)) options.Password = PasswordLookUp.ToValue(item);
            return options;
        }
    }
}
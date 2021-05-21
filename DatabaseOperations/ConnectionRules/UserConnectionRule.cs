using DatabaseOperations.DataTransferObjects;
using DatabaseOperations.Extensions;
using DatabaseOperations.Interfaces;

namespace DatabaseOperations.ConnectionRules
{
    internal class UserConnectionRule : IConnectionRule
    {
        private const string UserIdLookUp = "user id";

        public bool Check(string item)
        {
            return item.ToLower().Contains(UserIdLookUp);
        }

        public ConnectionOptions ApplyChange(ConnectionOptions options, string item)
        {
            if(string.IsNullOrWhiteSpace(options.UserId)) options.UserId = UserIdLookUp.ToValue(item);
            return options;
        }
    }
}
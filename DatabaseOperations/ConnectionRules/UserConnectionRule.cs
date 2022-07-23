namespace DatabaseOperations.ConnectionRules
{
    using DataTransferObjects;
    using Extensions;
    using Interfaces;

    internal class UserConnectionRule : IConnectionRule
    {
        private const string UserIdLookUp = "user id";

        public bool Check(string item)
        {
            return item.ToLower()
                .Contains(UserIdLookUp);
        }

        public ConnectionOptions ApplyChange(
            ConnectionOptions options,
            string item)
        {
            if (string.IsNullOrWhiteSpace(options.UserId)) options.UserId = UserIdLookUp.ToValue(item);
            return options;
        }

        public ConnectionProperties ApplyChange(
            ConnectionProperties properties,
            string item)
        {
            if (string.IsNullOrWhiteSpace(properties.UserId)) properties.UserId = UserIdLookUp.ToValue(item);
            return properties;
        }
    }
}
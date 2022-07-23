namespace DatabaseOperations.ConnectionRules
{
    using DataTransferObjects;
    using Extensions;
    using Interfaces;

    internal class AbbreviatedPasswordConnectionRule : IConnectionRule
    {
        private const string AbbreviatedPasswordLookUp = "pwd";

        public bool Check(string item)
        {
            return item.ToLower()
                .Contains(AbbreviatedPasswordLookUp);
        }

        public ConnectionOptions ApplyChange(
            ConnectionOptions options,
            string item)
        {
            if (string.IsNullOrWhiteSpace(options.Password)) options.Password = AbbreviatedPasswordLookUp.ToValue(item);
            return options;
        }

        public ConnectionProperties ApplyChange(
            ConnectionProperties properties,
            string item)
        {
            if (string.IsNullOrWhiteSpace(properties.Password)) properties.Password = AbbreviatedPasswordLookUp.ToValue(item);
            return properties;
        }
    }
}
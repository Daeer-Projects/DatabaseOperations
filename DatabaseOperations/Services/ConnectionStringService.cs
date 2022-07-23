namespace DatabaseOperations.Services
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using ConnectionRules;
    using DataTransferObjects;
    using Interfaces;

    internal static class ConnectionStringService
    {
        private static readonly char[] SplitArray = { ';' };
        private static bool isValid;

        private static readonly IList<IConnectionRule> ConnectionRules = new List<IConnectionRule>
        {
            new ApplicationConnectionRule(),
            new ConnectTimeoutConnectionRule(),
            new ConnectionTimeoutConnectionRule(),
            new DataSourceConnectionRule(),
            new ServerConnectionRule(),
            new AddressConnectionRule(),
            new AbbreviatedAddressConnectionRule(),
            new NetworkAddressConnectionRule(),
            new CatalogConnectionRule(),
            new DatabaseConnectionRule(),
            new SecurityConnectionRule(),
            new TrustedConnectionRule(),
            new PasswordConnectionRule(),
            new AbbreviatedPasswordConnectionRule(),
            new UserConnectionRule()
        };

        internal static ConnectionProperties ExtractConnectionParameters(string connectionString)
        {
            isValid = !string.IsNullOrWhiteSpace(connectionString);
            if (!isValid) return new ConnectionProperties();

            string[] itemArray = connectionString.Split(SplitArray, StringSplitOptions.RemoveEmptyEntries);

            ConnectionProperties connectionProperties = ProcessItemArray(itemArray);
            connectionProperties.ConnectionString = UpdateConnectionString(connectionProperties);
            return connectionProperties;
        }

        private static ConnectionProperties ProcessItemArray(IEnumerable<string> itemArray)
        {
            ConnectionProperties properties = new();
            foreach (string item in itemArray) ApplyConnectionRules(item, properties);

            return properties;
        }

        private static void ApplyConnectionRules(string item,
            ConnectionProperties properties)
        {
            foreach (IConnectionRule connectionRule in ConnectionRules)
                if (connectionRule.Check(item))
                    connectionRule.ApplyChange(properties, item);
        }

        private static string UpdateConnectionString(ConnectionProperties properties)
        {
            return !string.IsNullOrWhiteSpace(properties.ConnectTimeout)
                ? Regex.Replace(properties.ConnectionString, "Connect Timeout=[0-9]{1,3}", "Connect Timeout=5")
                : properties.ConnectionString;
        }
    }
}

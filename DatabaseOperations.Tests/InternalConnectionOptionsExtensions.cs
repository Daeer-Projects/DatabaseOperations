namespace DatabaseOperations.Tests
{
    using DatabaseOperations.DataTransferObjects;

    internal static class InternalConnectionOptionsExtensions
    {
        internal static ConnectionOptions ApplyApplicationName(
            this ConnectionOptions options,
            string applicationName)
        {
            options.ApplicationName = applicationName;
            return options;
        }

        internal static ConnectionOptions ApplyConnectTimeOut(
            this ConnectionOptions options,
            string timeout)
        {
            options.ConnectTimeout = timeout;
            return options;
        }

        internal static ConnectionOptions ApplyServer(
            this ConnectionOptions options,
            string serverName)
        {
            options.Server = serverName;
            return options;
        }

        internal static ConnectionOptions ApplyDatabaseName(
            this ConnectionOptions options,
            string databaseName)
        {
            options.DatabaseName = databaseName;
            return options;
        }

        internal static ConnectionOptions ApplyIntegratedSecurity(
            this ConnectionOptions options,
            string integratedSecurity)
        {
            options.IntegratedSecurity = integratedSecurity;
            return options;
        }

        internal static ConnectionOptions ApplyPassword(
            this ConnectionOptions options,
            string password)
        {
            options.Password = password;
            return options;
        }

        internal static ConnectionOptions ApplyUserId(
            this ConnectionOptions options,
            string userId)
        {
            options.UserId = userId;
            return options;
        }

        internal static ConnectionOptions OverrideConnectionString(
            this ConnectionOptions options,
            string connectionString)
        {
            options.ConnectionString = connectionString;
            return options;
        }
    }
}
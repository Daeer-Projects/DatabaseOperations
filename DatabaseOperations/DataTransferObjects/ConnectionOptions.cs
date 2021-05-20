using System;
using System.Data;
using System.Text.RegularExpressions;
using Microsoft.Data.SqlClient;
using Useful.Extensions;

namespace DatabaseOperations.DataTransferObjects
{
    public class ConnectionOptions
	{
        public ConnectionOptions(string connectionString, string backupPath, int timeout = 0)
        {
            InitialiseProperties(connectionString, backupPath, timeout);
        }

        private SqlParameter[] _parameters = Array.Empty<SqlParameter>();

        private const string ApplicationNameLookUp = "application name";
        private const string ConnectTimeoutLookUp = "connect timeout";
        private const string ConnectionTimeoutLookUp = "connection timeout";
        private const string DataSourceLookUp = "data source";
        private const string ServerLookUp = "server";
        private const string AddressLookUp = "address";
        private const string AbbreviatedAddressLookUp = "addr";
        private const string NetworkAddressLookUp = "network address";
        private const string InitialCatalogLookUp = "initial catalog";
        private const string DatabaseLookUp = "database";
        private const string IntegratedSecurityLookUp = "integrated security";
        private const string TrustedConnectionLookUp = "trusted_connection";
        private const string PasswordLookUp = "password";
        private const string AbbreviatedPasswordLookUp = "pwd";
        private const string UserIdLookUp = "user id";
        private const string EqualSymbol = "=";

        private readonly char[] _splitArray = {';'};

        public string ApplicationName { get; internal set; } = string.Empty;
        public string DatabaseName { get; internal set; } = string.Empty;
        public string ConnectTimeout { get; internal set; } = string.Empty;
        public string IntegratedSecurity { get; internal set; } = string.Empty;
        public string Password { get; internal set; } = string.Empty;
        public string Server { get; internal set; } = string.Empty;
        public string UserId { get; internal set; } = string.Empty;

        public string BackupLocation { get; private set; } = string.Empty;
        public string ConnectionString { get; internal set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public int CommandTimeout { get; private set; }

        public SqlParameter[] Parameters()
        {
            return _parameters;
        }

        public bool IsValid()
        {
            // ToDo: Create a new validator for this class and return the result of that.
            return true;
        }

        private void InitialiseProperties(string connectionString, string backupPath, int timeout)
        {
            var itemArray = connectionString.Split(_splitArray, StringSplitOptions.RemoveEmptyEntries);

            // ToDo: This method needs refactoring, as it is too complex.
            foreach (var item in itemArray)
            {
                switch (item)
                {
                    case { } itemValue when itemValue.ToLower().Contains(ApplicationNameLookUp):
                    {
                        ApplicationName = ToValue(ApplicationNameLookUp, itemValue);
                        break;
                    }
                    case { } itemValue when itemValue.ToLower().Contains(ConnectTimeoutLookUp):
                    {
                        if (string.IsNullOrWhiteSpace(ConnectTimeout)) ConnectTimeout = ToValue(ConnectTimeoutLookUp, itemValue);
                        break;
                    }
                    case { } itemValue when itemValue.ToLower().Contains(ConnectionTimeoutLookUp):
                    {
                        if (string.IsNullOrWhiteSpace(ConnectTimeout)) ConnectTimeout = ToValue(ConnectionTimeoutLookUp, itemValue);
                        break;
                    }
                    case { } itemValue when itemValue.ToLower().Contains(DataSourceLookUp):
                    {
                        if (string.IsNullOrWhiteSpace(Server)) Server = ToValue(DataSourceLookUp, itemValue);
                        break;
                    }
                    case { } itemValue when itemValue.ToLower().Contains(ServerLookUp):
                    {
                        if (string.IsNullOrWhiteSpace(Server)) Server = ToValue(ServerLookUp, itemValue);
                        break;
                    }
                    case { } itemValue when itemValue.ToLower().Contains(AddressLookUp):
                    {
                        if (string.IsNullOrWhiteSpace(Server)) Server = ToValue(AddressLookUp, itemValue);
                        break;
                    }
                    case { } itemValue when itemValue.ToLower().Contains(AbbreviatedAddressLookUp):
                    {
                        if (string.IsNullOrWhiteSpace(Server)) Server = ToValue(AbbreviatedAddressLookUp, itemValue);
                        break;
                    }
                    case { } itemValue when itemValue.ToLower().Contains(NetworkAddressLookUp):
                    {
                        if (string.IsNullOrWhiteSpace(Server)) Server = ToValue(NetworkAddressLookUp, itemValue);
                        break;
                    }
                    case { } itemValue when itemValue.ToLower().Contains(InitialCatalogLookUp):
                    {
                        if (string.IsNullOrWhiteSpace(DatabaseName)) DatabaseName = ToValue(InitialCatalogLookUp, itemValue);
                        break;
                    }
                    case { } itemValue when itemValue.ToLower().Contains(DatabaseLookUp):
                    {
                        if (string.IsNullOrWhiteSpace(DatabaseName)) DatabaseName = ToValue(DatabaseLookUp, itemValue);
                        break;
                    }
                    case { } itemValue when itemValue.ToLower().Contains(IntegratedSecurityLookUp):
                    {
                        if (string.IsNullOrWhiteSpace(IntegratedSecurity)) IntegratedSecurity = ToValue(IntegratedSecurityLookUp, itemValue);
                        break;
                    }
                    case { } itemValue when itemValue.ToLower().Contains(TrustedConnectionLookUp):
                    {
                        if (string.IsNullOrWhiteSpace(IntegratedSecurity)) IntegratedSecurity = ToValue(TrustedConnectionLookUp, itemValue);
                        break;
                    }
                    case { } itemValue when itemValue.ToLower().Contains(PasswordLookUp):
                    {
                        if (string.IsNullOrWhiteSpace(Password)) Password = ToValue(PasswordLookUp, itemValue);
                        break;
                    }
                    case { } itemValue when itemValue.ToLower().Contains(AbbreviatedPasswordLookUp):
                    {
                        if (string.IsNullOrWhiteSpace(Password)) Password = ToValue(AbbreviatedPasswordLookUp, itemValue);
                        break;
                    }
                    case { } itemValue when itemValue.ToLower().Contains(UserIdLookUp):
                    {
                        if (string.IsNullOrWhiteSpace(UserId)) UserId = ToValue(UserIdLookUp, itemValue);
                        break;
                    }
                }
            }

            var location = $"{backupPath}{DatabaseName}_Full_{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.bak";
            var description = $"Full backup of the `{DatabaseName}` database.";

            var updatedConnectionString = !string.IsNullOrWhiteSpace(ConnectTimeout)
                ? Regex.Replace(connectionString, "Connect Timeout=[0-9]{1,3}", "Connect Timeout=5")
                : connectionString;
            
            ConnectionString = updatedConnectionString;
            BackupLocation = location;
            Description = description;
            CommandTimeout = SetDefaultOrTimeout(timeout);
            _parameters = GetParameters(DatabaseName, location, description, backupPath);
        }

        private static string ToValue(string key, string value)
        {
            var searchString = key + EqualSymbol;
            var newValue = value.SubstringAfterValue(searchString);
            return newValue;
        }
        
        private static int SetDefaultOrTimeout(int timeout)
        {
            return timeout == 0 ? 60 * 60 : timeout;
        }

        private static SqlParameter[] GetParameters(string database, string location, string description, string backupPath)
        {
            var nameParameter = new SqlParameter(Constants.Parameters.NameParameter, SqlDbType.VarChar) { Value = database };
            var locationParameter = new SqlParameter(Constants.Parameters.LocationParameter, SqlDbType.VarChar) { Value = location };
            var descriptionParameter = new SqlParameter(Constants.Parameters.DescriptionParameter, SqlDbType.VarChar) { Value = description };
            var pathParameter = new SqlParameter(Constants.Parameters.PathParameter, SqlDbType.VarChar) { Value = backupPath };

            return new[] { nameParameter, locationParameter, descriptionParameter, pathParameter };
        }
	}
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using DatabaseOperations.ConnectionRules;
using DatabaseOperations.Extensions;
using DatabaseOperations.Interfaces;
using DatabaseOperations.Validators;
using Microsoft.Data.SqlClient;

namespace DatabaseOperations.DataTransferObjects
{
    /// <summary>
    /// The options used for connecting to the database.
    /// </summary>
    public class ConnectionOptions
	{
        /// <summary>
        /// Initialises a new instance of the ConnectionOptions.
        /// </summary>
        /// <param name="connectionString">
        /// The connection details for the database.
        /// </param>
        /// <param name="backupPath">
        /// The location where the backup is going to be stored.
        /// </param>
        /// <param name="timeout">
        /// The <paramref name="timeout"/> of the execution process, not the connection to the
        /// database <paramref name="timeout"/>.
        /// </param>
        public ConnectionOptions(string connectionString, string backupPath, int timeout = 0)
        {
            InitialiseProperties(connectionString, backupPath, timeout);
        }

        private SqlParameter[] _parameters = Array.Empty<SqlParameter>();

        private readonly char[] _splitArray = {';'};
        private bool _isValid;

        private readonly IList<IConnectionRule> _connectionRules = new List<IConnectionRule>
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

        internal string ApplicationName { get; set; } = string.Empty;
        internal string DatabaseName { get; set; } = string.Empty;
        internal string ConnectTimeout { get; set; } = string.Empty;
        internal string IntegratedSecurity { get; set; } = string.Empty;
        internal string Password { get; set; } = string.Empty;
        internal string Server { get; set; } = string.Empty;
        internal string UserId { get; set; } = string.Empty;

        internal string BackupLocation { get; private set; } = string.Empty;
        internal string ConnectionString { get; set; } = string.Empty;
        internal string Description { get; private set; } = string.Empty;
        internal int CommandTimeout { get; private set; }
        internal IList<string> Messages { get; } = new List<string>();

        internal SqlParameter[] Parameters()
        {
            return _parameters;
        }

        internal bool IsValid()
        {
            // ReSharper disable once InvertIf
            if (_isValid)
            {
                var validationResults = this.CheckValidation(new ConnectionOptionsValidator());
                _isValid = validationResults.IsValid;
                foreach (var validationResultsError in validationResults.Errors)
                {
                    Messages.Add(validationResultsError.ErrorMessage);
                }
            }
            
            return _isValid;
        }

        private void InitialiseProperties(string connectionString, string backupPath, int timeout)
        {
            _isValid = !string.IsNullOrWhiteSpace(connectionString);
            if (!_isValid) return;
            
            string[] itemArray = connectionString.Split(_splitArray, StringSplitOptions.RemoveEmptyEntries);

            ProcessItemArray(itemArray);

            var location = $"{backupPath}{DatabaseName}_Full_{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.bak";
            var description = $"Full backup of the `{DatabaseName}` database.";

            ConnectionString = UpdateConnectionString(connectionString);
            BackupLocation = location;
            Description = description;
            CommandTimeout = SetDefaultOrTimeout(timeout);
            _parameters = GetParameters(DatabaseName, location, description, backupPath);
        }

        private void ProcessItemArray(IEnumerable<string> itemArray)
        {
            foreach (string item in itemArray)
            {
                ApplyConnectionRules(item);
            }
        }

        private void ApplyConnectionRules(string item)
        {
            foreach (var connectionRule in _connectionRules)
            {
                if (connectionRule.Check(item)) connectionRule.ApplyChange(this, item);
            }
        }

        private string UpdateConnectionString(string connectionString)
        {
            return !string.IsNullOrWhiteSpace(ConnectTimeout)
                ? Regex.Replace(connectionString, "Connect Timeout=[0-9]{1,3}", "Connect Timeout=5")
                : connectionString;
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

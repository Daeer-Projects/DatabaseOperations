namespace DatabaseOperations.DataTransferObjects
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Text.RegularExpressions;
    using ConnectionRules;
    using Constants;
    using Extensions;
    using FluentValidation.Results;
    using Interfaces;
    using Microsoft.Data.SqlClient;
    using Useful.Extensions;
    using Validators;
    using Wrappers;

    /// <summary>
    ///     The options used for connecting to the database.
    /// </summary>
    public class ConnectionOptions
    {
        /// <summary>
        ///     Initialises a new instance of the ConnectionOptions.
        /// </summary>
        /// <param name="connectionString">
        ///     The connection details for the database.
        /// </param>
        /// <param name="backupPath">
        ///     The location where the backup is going to be stored.
        /// </param>
        /// <param name="timeout">
        ///     The <paramref name="timeout" /> of the execution process, not the connection to the
        ///     database <paramref name="timeout" />.
        /// </param>
        public ConnectionOptions(
            string connectionString,
            string backupPath = "",
            int timeout = 0)
        {
            dateTimeWrapper = new DateTimeWrapper();
            InitialiseProperties(connectionString, backupPath, timeout);
        }

        internal ConnectionOptions(
            string connectionString,
            IDateTimeWrapper dateWrapper,
            string backupPath = "",
            int timeout = 0)
        {
            dateTimeWrapper = dateWrapper;
            InitialiseProperties(connectionString, backupPath, timeout);
        }

        private readonly IList<IConnectionRule> connectionRules = new List<IConnectionRule>
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

        private readonly IDateTimeWrapper dateTimeWrapper;

        private readonly char[] splitArray = { ';' };
        private SqlParameter[] backupParameters = Array.Empty<SqlParameter>();

        private SqlParameter[] executionParameters = Array.Empty<SqlParameter>();
        private bool isValid;

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

        internal SqlParameter[] ExecutionParameters()
        {
            return executionParameters;
        }

        internal SqlParameter[] BackupParameters()
        {
            return backupParameters;
        }

        internal bool IsValid()
        {
            // ReSharper disable once InvertIf
            if (isValid)
            {
                ValidationResult validationResults = this.CheckValidation(new ConnectionOptionsValidator());
                isValid = validationResults.IsValid;
                foreach (ValidationFailure? validationResultsError in validationResults.Errors)
                    Messages.Add(validationResultsError.ErrorMessage);
            }

            return isValid;
        }

        internal void RemovePathFromBackupLocation()
        {
            string? startOfPath = BackupLocation.SubstringBeforeValue(DatabaseName);
            string? requiredLocation = BackupLocation.SubstringAfterValue(startOfPath);
            BackupLocation = requiredLocation;
            executionParameters = GetParameters(DatabaseName, BackupLocation, Description);
        }

        private void InitialiseProperties(
            string connectionString,
            string backupPath,
            int timeout)
        {
            isValid = !string.IsNullOrWhiteSpace(connectionString);
            if (!isValid) return;

            string[] itemArray = connectionString.Split(splitArray, StringSplitOptions.RemoveEmptyEntries);

            ProcessItemArray(itemArray);

            string location = $"{backupPath}{DatabaseName}_Full_{dateTimeWrapper.Now:yyyy-MM-dd-HH-mm-ss}.bak";
            string description = $"Full backup of the `{DatabaseName}` database.";

            ConnectionString = UpdateConnectionString(connectionString);
            BackupLocation = location;
            Description = description;
            CommandTimeout = SetDefaultOrTimeout(timeout);
            backupParameters = GetBackupParameters(backupPath);
            executionParameters = GetParameters(DatabaseName, location, description);
        }

        private void ProcessItemArray(IEnumerable<string> itemArray)
        {
            foreach (string item in itemArray) ApplyConnectionRules(item);
        }

        private void ApplyConnectionRules(string item)
        {
            foreach (IConnectionRule connectionRule in connectionRules)
                if (connectionRule.Check(item))
                    connectionRule.ApplyChange(this, item);
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

        private static SqlParameter[] GetBackupParameters(string backupPath)
        {
            SqlParameter pathParameter = new(Parameters.PathParameter, SqlDbType.VarChar) { Value = backupPath };

            return new[] { pathParameter };
        }

        private static SqlParameter[] GetParameters(
            string database,
            string location,
            string description)
        {
            SqlParameter nameParameter = new(Parameters.NameParameter, SqlDbType.VarChar) { Value = database };
            SqlParameter locationParameter = new(Parameters.LocationParameter, SqlDbType.VarChar)
                { Value = location };
            SqlParameter descriptionParameter = new(Parameters.DescriptionParameter, SqlDbType.VarChar)
                { Value = description };

            return new[] { nameParameter, locationParameter, descriptionParameter };
        }
    }
}
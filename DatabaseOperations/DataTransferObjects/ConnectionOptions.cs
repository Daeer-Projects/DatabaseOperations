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

        private SqlParameter[] _parameters;

        public string DatabaseName { get; private set; }
        public string ConnectionString { get; private set; }
        public string BackupLocation { get; private set; }
        public string Description { get; private set; }
        public int CommandTimeout { get; private set; }

        public SqlParameter[] Parameters()
        {
            return _parameters;
        }

        private void InitialiseProperties(string connectionString, string backupPath, int timeout)
        {
            var itemArray = connectionString.Split(';');

            var database = itemArray[1].SubstringAfterValue("Database=");
            var location = $"{backupPath}{database}_Full_{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.bak";
            var description = $"Full backup of the `{database}` database.";
            var updatedConnectionString = Regex.Replace(connectionString, "Connect Timeout=[0-9]{1,3}", "Connect Timeout=5");

            DatabaseName = database;
            ConnectionString = updatedConnectionString;
            BackupLocation = location;
            Description = description;
            CommandTimeout = SetDefaultOrTimeout(timeout);
            _parameters = GetParameters(database, location, description, backupPath);
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

namespace DatabaseOperations.Services
{
    using System.Data;
    using Constants;
    using DataTransferObjects;
    using Interfaces;
    using Microsoft.Data.SqlClient;
    using Options;
    using Wrappers;

    internal static class BackupParameterService
    {
        internal static BackupProperties GetBackupProperties(
            ConnectionProperties connectionOptions,
            OperatorOptions operatorOptions)
        {
            IDateTimeWrapper dateTimeWrapper = new DateTimeWrapper();
            return GetBackupProperties(connectionOptions, operatorOptions, dateTimeWrapper);
        }

        internal static BackupProperties GetBackupPropertiesForTests(
            ConnectionProperties connectionOptions,
            OperatorOptions operatorOptions,
            IDateTimeWrapper dateTimeWrapper
        )
        {
            return GetBackupProperties(connectionOptions, operatorOptions, dateTimeWrapper);
        }

        private static BackupProperties GetBackupProperties(
            ConnectionProperties connectionOptions,
            OperatorOptions operatorOptions,
            IDateTimeWrapper dateTimeWrapper)
        {
            BackupProperties backupProperties = new();

            string location =
                $"{operatorOptions.BackupPath}{connectionOptions.DatabaseName}_Full_{dateTimeWrapper.Now:yyyy-MM-dd-HH-mm-ss}.bak";
            string description = $"Full backup of the `{connectionOptions.DatabaseName}` database.";

            backupProperties.BackupLocation = location;
            backupProperties.Description = description;
            backupProperties.BackupPath = operatorOptions.BackupPath;
            backupProperties.CommandTimeout = SetDefaultOrTimeout(operatorOptions.Timeout);
            backupProperties.BackupParameters = GetBackupParameters(operatorOptions.BackupPath);
            backupProperties.ExecutionParameters = GetParameters(
                connectionOptions.DatabaseName,
                backupProperties.BackupLocation,
                backupProperties.Description);

            return backupProperties;
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
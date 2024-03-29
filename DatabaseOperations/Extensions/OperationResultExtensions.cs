﻿namespace DatabaseOperations.Extensions
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using DataTransferObjects;
    using FluentValidation.Results;
    using Interfaces;
    using Services;
    using Validators;

    internal static class OperationResultExtensions
    {
        private const string ExecutionCancelledMessage = "Cancel called on the token.";
        private const string BackupPathCheckFailureMessage = "Unable to check the path, reverting to default save path.";

        internal static OperationResult ValidateConnectionOptions(
            this OperationResult operationResult,
            ConnectionOptions options)
        {
            if (options.IsValid()) return operationResult;

            operationResult.Result = false;
            operationResult.Messages = options.Messages;
            return operationResult;
        }

        internal static OperationResult ValidateConnectionProperties(
            this OperationResult operationResult,
            ConnectionProperties properties)
        {
            ValidationResult validationResult = properties.CheckValidation(new ConnectionPropertiesValidator());

            operationResult.Result = validationResult.IsValid;
            if (validationResult.IsValid) return operationResult;

            foreach (ValidationFailure validationResultError in validationResult.Errors)
                operationResult.Messages.Add(validationResultError.ErrorMessage);

            return operationResult;
        }

        internal static OperationResult ExecuteBackupPath(
            this OperationResult operationResult,
            ConnectionOptions options,
            ISqlExecutor sqlExecutor)
        {
            return !operationResult.Result ? operationResult : sqlExecutor.ExecuteBackupPath(operationResult, options);
        }

        internal static OperationResult ExecuteBackupPath(
            this OperationResult operationResult,
            ConnectionProperties connectionProperties,
            BackupProperties backupProperties,
            ISqlExecutor sqlExecutor)
        {
            return !operationResult.Result
                ? operationResult
                : sqlExecutor.ExecuteBackupPath(operationResult, connectionProperties, backupProperties);
        }

        internal static OperationResult CheckBackupPathExecution(
            this OperationResult operationResult,
            ConnectionOptions options)
        {
            if (!operationResult.Messages.Any()) return operationResult;

            operationResult.Result = true;
            operationResult.Messages.Add(BackupPathCheckFailureMessage);
            options.RemovePathFromBackupLocation();

            return operationResult;
        }

        internal static OperationResult CheckBackupPathExecution(
            this OperationResult operationResult,
            ConnectionProperties connectionProperties,
            BackupProperties backupProperties)
        {
            if (!operationResult.Messages.Any()) return operationResult;

            operationResult.Result = true;
            operationResult.Messages.Add(BackupPathCheckFailureMessage);
            backupProperties.SetExecutorToUseFileNameOnly(connectionProperties);

            return operationResult;
        }

        internal static OperationResult ExecuteBackup(
            this OperationResult operationResult,
            ConnectionOptions options,
            ISqlExecutor sqlExecutor)
        {
            return !operationResult.Result ? operationResult : sqlExecutor.ExecuteBackupDatabase(operationResult, options);
        }

        internal static OperationResult ExecuteBackup(
            this OperationResult operationResult,
            ConnectionProperties connectionProperties,
            BackupProperties backupProperties,
            ISqlExecutor sqlExecutor)
        {
            return !operationResult.Result
                ? operationResult
                : sqlExecutor.ExecuteBackupDatabase(operationResult, connectionProperties, backupProperties);
        }

        internal static async Task<OperationResult> ValidateConnectionOptionsAsync(
            this OperationResult operationResult,
            ConnectionOptions options)
        {
            if (options.IsValid())
                return await Task.FromResult(operationResult)
                    .ConfigureAwait(false);

            operationResult.Result = false;
            operationResult.Messages = options.Messages;
            return await Task.FromResult(operationResult)
                .ConfigureAwait(false);
        }

        internal static async Task<OperationResult> ValidateConnectionPropertiesAsync(
            this OperationResult operationResult,
            ConnectionProperties properties)
        {
            ValidationResult validationResult = properties.CheckValidation(new ConnectionPropertiesValidator());

            operationResult.Result = validationResult.IsValid;
            if (validationResult.IsValid)
                return await Task.FromResult(operationResult)
                    .ConfigureAwait(false);

            foreach (ValidationFailure validationResultError in validationResult.Errors)
                operationResult.Messages.Add(validationResultError.ErrorMessage);

            return await Task.FromResult(operationResult)
                .ConfigureAwait(false);
        }

        internal static async Task<OperationResult> CheckForCancellation(
            this Task<OperationResult> operationResult,
            CancellationToken token)
        {
            if (!token.IsCancellationRequested) return await operationResult.ConfigureAwait(false);

            OperationResult result = await operationResult.ConfigureAwait(false);
            result.Result = false;
            if (!result.Messages.Contains(ExecutionCancelledMessage)) result.Messages.Add(ExecutionCancelledMessage);
            return await Task.FromResult(result)
                .ConfigureAwait(false);
        }

        internal static async Task<OperationResult> ExecuteBackupPathAsync(
            this Task<OperationResult> operationResult,
            ConnectionOptions options,
            CancellationToken token,
            ISqlExecutor sqlExecutor)
        {
            OperationResult result = await operationResult.ConfigureAwait(false);
            return !result.Result
                ? await Task.FromResult(result)
                    .ConfigureAwait(false)
                : await sqlExecutor.ExecuteBackupPathAsync(result, options, token)
                    .ConfigureAwait(false);
        }

        internal static async Task<OperationResult> ExecuteBackupPathAsync(
            this Task<OperationResult> operationResult,
            ConnectionProperties connectionProperties,
            BackupProperties backupProperties,
            CancellationToken token,
            ISqlExecutor sqlExecutor)
        {
            OperationResult result = await operationResult.ConfigureAwait(false);
            return !result.Result
                ? await Task.FromResult(result)
                    .ConfigureAwait(false)
                : await sqlExecutor.ExecuteBackupPathAsync(
                        result,
                        connectionProperties,
                        backupProperties,
                        token)
                    .ConfigureAwait(false);
        }

        internal static async Task<OperationResult> CheckBackupPathExecutionAsync(
            this Task<OperationResult> operationResult,
            ConnectionOptions options,
            CancellationToken token)
        {
            OperationResult result = await operationResult.ConfigureAwait(false);
            if (!result.Messages.Any() || token.IsCancellationRequested)
                return await Task.FromResult(result)
                    .ConfigureAwait(false);

            result.Result = true;
            result.Messages.Add(BackupPathCheckFailureMessage);
            options.RemovePathFromBackupLocation();

            return await Task.FromResult(result)
                .ConfigureAwait(false);
        }

        internal static async Task<OperationResult> CheckBackupPathExecutionAsync(
            this Task<OperationResult> operationResult,
            ConnectionProperties connectionProperties,
            BackupProperties backupProperties,
            CancellationToken token)
        {
            OperationResult result = await operationResult.ConfigureAwait(false);
            if (!result.Messages.Any() || token.IsCancellationRequested)
                return await Task.FromResult(result)
                    .ConfigureAwait(false);

            result.Result = true;
            result.Messages.Add(BackupPathCheckFailureMessage);
            backupProperties.SetExecutorToUseFileNameOnly(connectionProperties);

            return await Task.FromResult(result)
                .ConfigureAwait(false);
        }

        internal static async Task<OperationResult> ExecuteBackupAsync(
            this Task<OperationResult> operationResult,
            ConnectionOptions options,
            CancellationToken token,
            ISqlExecutor sqlExecutor)
        {
            OperationResult result = await operationResult.ConfigureAwait(false);

            return !result.Result
                ? await Task.FromResult(result)
                    .ConfigureAwait(false)
                : await sqlExecutor.ExecuteBackupDatabaseAsync(result, options, token)
                    .ConfigureAwait(false);
        }

        internal static async Task<OperationResult> ExecuteBackupAsync(
            this Task<OperationResult> operationResult,
            ConnectionProperties connectionProperties,
            BackupProperties backupProperties,
            CancellationToken token,
            ISqlExecutor sqlExecutor)
        {
            OperationResult result = await operationResult.ConfigureAwait(false);

            return !result.Result
                ? await Task.FromResult(result)
                    .ConfigureAwait(false)
                : await sqlExecutor.ExecuteBackupDatabaseAsync(
                        result,
                        connectionProperties,
                        backupProperties,
                        token)
                    .ConfigureAwait(false);
        }
    }
}
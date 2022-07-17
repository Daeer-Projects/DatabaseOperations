namespace DatabaseOperations.Wrappers
{
    using System.Threading;
    using System.Threading.Tasks;
    using Interfaces;
    using Microsoft.Data.SqlClient;

    internal sealed class SqlConnectionWrapper : ISqlConnectionWrapper
    {
        internal SqlConnectionWrapper(string connectionString)
        {
            sqlConnection = new SqlConnection(connectionString);
        }

        private readonly SqlConnection sqlConnection;

        public SqlConnection Get()
        {
            return sqlConnection;
        }

        public void Open()
        {
            sqlConnection.Open();
        }

        public async Task OpenAsync(CancellationToken token)
        {
            await sqlConnection.OpenAsync(token)
                .ConfigureAwait(false);
        }

        public void Dispose()
        {
            sqlConnection.Dispose();
        }
    }
}
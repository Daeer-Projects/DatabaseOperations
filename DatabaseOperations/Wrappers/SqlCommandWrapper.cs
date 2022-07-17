namespace DatabaseOperations.Wrappers
{
    using System.Data;
    using System.Threading;
    using System.Threading.Tasks;
    using Interfaces;
    using Microsoft.Data.SqlClient;

    internal sealed class SqlCommandWrapper : ISqlCommandWrapper
    {
        internal SqlCommandWrapper(
            string commandText,
            ISqlConnectionWrapper sqlConnection)
        {
            sqlCommand = new SqlCommand(commandText, sqlConnection.Get()) { CommandType = CommandType.Text };
        }

        private readonly SqlCommand sqlCommand;

        public void AddParameters(SqlParameter[] parameters)
        {
            sqlCommand.Parameters.AddRange(parameters);
        }

        public int ExecuteNonQuery()
        {
            return sqlCommand.ExecuteNonQuery();
        }

        public async Task<int> ExecuteNonQueryAsync(CancellationToken token)
        {
            return await sqlCommand.ExecuteNonQueryAsync(token)
                .ConfigureAwait(false);
        }

        public void SetCommandTimeout(int timeout)
        {
            sqlCommand.CommandTimeout = timeout;
        }

        public void Dispose()
        {
            sqlCommand.Dispose();
        }

        public SqlCommand Get()
        {
            return sqlCommand;
        }
    }
}
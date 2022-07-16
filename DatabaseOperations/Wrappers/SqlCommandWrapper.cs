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
            _sqlCommand = new SqlCommand(commandText, sqlConnection.Get()) { CommandType = CommandType.Text };
        }

        private readonly SqlCommand _sqlCommand;

        public void AddParameters(SqlParameter[] parameters)
        {
            _sqlCommand.Parameters.AddRange(parameters);
        }

        public int ExecuteNonQuery()
        {
            return _sqlCommand.ExecuteNonQuery();
        }

        public async Task<int> ExecuteNonQueryAsync(CancellationToken token)
        {
            return await _sqlCommand.ExecuteNonQueryAsync(token)
                .ConfigureAwait(false);
        }

        public void SetCommandTimeout(int timeout)
        {
            _sqlCommand.CommandTimeout = timeout;
        }

        public void Dispose()
        {
            _sqlCommand.Dispose();
        }

        public SqlCommand Get()
        {
            return _sqlCommand;
        }
    }
}
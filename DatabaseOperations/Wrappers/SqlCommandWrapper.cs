using System.Data;
using DatabaseOperations.Interfaces;
using Microsoft.Data.SqlClient;

namespace DatabaseOperations.Wrappers
{
    public sealed class SqlCommandWrapper : ISqlCommandWrapper
    {
        public SqlCommandWrapper(string commandText, ISqlConnectionWrapper sqlConnection)
        {
            _sqlCommand = new SqlCommand(commandText, sqlConnection.Get()) { CommandType = CommandType.Text };
        }

        private readonly SqlCommand _sqlCommand;

        public SqlCommand Get()
        {
            return _sqlCommand;
        }

        public void AddParameters(SqlParameter[] parameters)
        {
            _sqlCommand.Parameters.AddRange(parameters);
        }

        public int ExecuteNonQuery()
        {
            return _sqlCommand.ExecuteNonQuery();
        }

        public void SetCommandTimeout(int timeout)
        {
            _sqlCommand.CommandTimeout = timeout;
        }

        public void Dispose()
        {
            _sqlCommand.Dispose();
        }
	}
}

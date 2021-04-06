using DatabaseOperations.Interfaces;
using Microsoft.Data.SqlClient;

namespace DatabaseOperations.Wrappers
{
    public class SqlConnectionWrapper : ISqlConnectionWrapper
	{
        public SqlConnectionWrapper(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
        }

        private readonly SqlConnection _connection;

        public SqlConnection GetConnection()
        {
            return _connection;
        }
	}
}

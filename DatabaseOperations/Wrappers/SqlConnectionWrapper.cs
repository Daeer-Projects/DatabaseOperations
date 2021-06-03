using DatabaseOperations.Interfaces;
using Microsoft.Data.SqlClient;

namespace DatabaseOperations.Wrappers
{
    internal sealed class SqlConnectionWrapper : ISqlConnectionWrapper
	{
        internal SqlConnectionWrapper(string connectionString)
        {
            _sqlConnection = new SqlConnection(connectionString);
        }

        private readonly SqlConnection _sqlConnection;

        public SqlConnection Get()
        {
            return _sqlConnection;
        }

        public void Open()
        {
            _sqlConnection.Open();
        }

        public void Dispose()
        {
            _sqlConnection.Dispose();
        }
	}
}

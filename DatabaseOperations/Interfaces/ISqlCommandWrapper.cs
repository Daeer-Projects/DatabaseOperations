using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace DatabaseOperations.Interfaces
{
    internal interface ISqlCommandWrapper : IDisposable
    {
	    void AddParameters(SqlParameter[] parameters);
        void SetCommandTimeout(int timeout);
        int ExecuteNonQuery();
        Task<int> ExecuteNonQueryAsync(CancellationToken token);
    }
}

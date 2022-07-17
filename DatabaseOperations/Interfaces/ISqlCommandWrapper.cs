namespace DatabaseOperations.Interfaces
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Data.SqlClient;

    internal interface ISqlCommandWrapper : IDisposable
    {
        void AddParameters(SqlParameter[] parameters);
        void SetCommandTimeout(int timeout);
        int ExecuteNonQuery();
        Task<int> ExecuteNonQueryAsync(CancellationToken token);
    }
}
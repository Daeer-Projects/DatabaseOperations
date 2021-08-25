using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace DatabaseOperations.Interfaces
{
    internal interface ISqlConnectionWrapper : IDisposable
    {
        SqlConnection Get();

        void Open();

        Task OpenAsync(CancellationToken token);
    }
}

namespace DatabaseOperations.Interfaces
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Data.SqlClient;

    internal interface ISqlConnectionWrapper : IDisposable
    {
        SqlConnection Get();

        void Open();

        Task OpenAsync(CancellationToken token);
    }
}
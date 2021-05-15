using System;
using Microsoft.Data.SqlClient;

namespace DatabaseOperations.Interfaces
{
    public interface ISqlConnectionWrapper : IDisposable
    {
        SqlConnection Get();
        void Open();
    }
}

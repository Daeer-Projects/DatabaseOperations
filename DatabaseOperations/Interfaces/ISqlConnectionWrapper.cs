using System;
using Microsoft.Data.SqlClient;

namespace DatabaseOperations.Interfaces
{
    internal interface ISqlConnectionWrapper : IDisposable
    {
        SqlConnection Get();
        void Open();
    }
}

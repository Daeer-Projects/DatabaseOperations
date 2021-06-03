using System;
using Microsoft.Data.SqlClient;

namespace DatabaseOperations.Interfaces
{
    internal interface ISqlCommandWrapper : IDisposable
    {
	    void AddParameters(SqlParameter[] parameters);
        void SetCommandTimeout(int timeout);
        int ExecuteNonQuery();
    }
}

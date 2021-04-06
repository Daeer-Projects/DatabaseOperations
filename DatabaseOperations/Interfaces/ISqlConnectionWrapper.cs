using Microsoft.Data.SqlClient;

namespace DatabaseOperations.Interfaces
{
    public interface ISqlConnectionWrapper
    {
        SqlConnection GetConnection();
    }
}

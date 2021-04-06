using Microsoft.SqlServer.Management.Common;

namespace DatabaseOperations.Interfaces
{
    public interface IServerConnectionWrapper
    {
        ServerConnection GetServerConnection();
    }
}

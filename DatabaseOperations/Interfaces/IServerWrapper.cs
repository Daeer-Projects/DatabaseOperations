using Microsoft.SqlServer.Management.Smo;

namespace DatabaseOperations.Interfaces
{
    public interface IServerWrapper
    {
        Server GetServer();
    }
}

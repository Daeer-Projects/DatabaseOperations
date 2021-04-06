using DatabaseOperations.Interfaces;
using Microsoft.SqlServer.Management.Common;

namespace DatabaseOperations.Wrappers
{
    public class ServerConnectionWrapper : IServerConnectionWrapper
	{
        public ServerConnectionWrapper(ISqlConnectionWrapper connectionWrapper)
        {
            _serverConnection = new ServerConnection(connectionWrapper.GetConnection());
        }

        private readonly ServerConnection _serverConnection;

        public ServerConnection GetServerConnection()
        {
            return _serverConnection;
        }
	}
}

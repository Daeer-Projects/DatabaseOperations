using DatabaseOperations.Interfaces;
using Microsoft.SqlServer.Management.Smo;

namespace DatabaseOperations.Wrappers
{
    public class ServerWrapper : IServerWrapper
	{
        public ServerWrapper(IServerConnectionWrapper connection)
        {
            _server = new Server(connection.GetServerConnection());
        }

        private readonly Server _server;

        public Server GetServer()
        {
            return _server;
        }
	}
}

using System;
using DatabaseOperations.DataTransferObjects;

namespace DatabaseOperations.Extensions
{
    public static class ConnectionDetailsExtensions
    {
        public static bool IsLocalServer(this ConnectionDetails details)
        {
            var isLocaldb = details.ServerName.ToLower().Contains("localdb");
            var isLocalInstance = details.ServerName.Contains(".\\");
            var isLocalNamedInstance = details.ServerName.Contains(Environment.MachineName);
            var result = isLocaldb || isLocalInstance || isLocalNamedInstance;
            return result;
        }
	}
}

﻿using DatabaseOperations.DataTransferObjects;
using DatabaseOperations.Extensions;
using DatabaseOperations.Interfaces;

namespace DatabaseOperations.ConnectionRules
{
    internal class ServerConnectionRule : IConnectionRule
    {
        private const string ServerLookUp = "server";

        public bool Check(string item)
        {
            return item.ToLower().Contains(ServerLookUp);
        }

        public ConnectionOptions ApplyChange(ConnectionOptions options, string item)
        {
            if(string.IsNullOrWhiteSpace(options.Server)) options.Server = ServerLookUp.ToValue(item);
            return options;
        }
    }
}
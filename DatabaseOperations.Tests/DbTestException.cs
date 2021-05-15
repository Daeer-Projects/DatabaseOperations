using System;
using System.Data.Common;

namespace DatabaseOperations.Tests
{
    public class DbTestException : DbException
    {
        public DbTestException()
        { }
        
        public DbTestException(string errorMessage)
        {
            Message = errorMessage;
        }

        public DbTestException(string errorMessage, Exception _)
        {
            Message = errorMessage;
        }

        public override string Message { get; } = string.Empty;
    }
}

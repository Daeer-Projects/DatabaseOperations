using System.Data.Common;

namespace DatabaseOperations.Tests
{
    public class DbTestException : DbException
    {
        public DbTestException(string errorMessage)
        {
            Message = errorMessage;
        }

        public override string Message { get; } = string.Empty;
    }
}

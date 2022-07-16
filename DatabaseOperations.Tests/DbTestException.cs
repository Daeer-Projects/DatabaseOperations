namespace DatabaseOperations.Tests
{
    using System.Data.Common;

    public class DbTestException : DbException
    {
        public DbTestException(string errorMessage)
        {
            Message = errorMessage;
        }

        public override string Message { get; } = string.Empty;
    }
}
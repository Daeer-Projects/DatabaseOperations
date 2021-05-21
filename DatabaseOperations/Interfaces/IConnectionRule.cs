using DatabaseOperations.DataTransferObjects;

namespace DatabaseOperations.Interfaces
{
    internal interface IConnectionRule
    {
        bool Check(string item);
        ConnectionOptions ApplyChange(ConnectionOptions options, string item);
    }
}
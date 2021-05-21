using System.Collections.Generic;

namespace DatabaseOperations.DataTransferObjects
{
    public class OperationResult<T> where T : new()
    {
        public T Result { get; set; } = new();
        public IList<string> Messages { get; set; } = new List<string>();
    }
}

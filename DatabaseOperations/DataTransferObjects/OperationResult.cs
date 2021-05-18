using System.Collections.Generic;

namespace DatabaseOperations.DataTransferObjects
{
    public class OperationResult<T>
    {
        public T Result { get; set; }
        public IList<string> Messages { get; set; } = new List<string>();
    }
}

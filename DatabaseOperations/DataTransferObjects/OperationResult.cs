using System.Collections.Generic;

namespace DatabaseOperations.DataTransferObjects
{
    public class OperationResult<T> where T : class 
    {
        public T Result { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public bool IsValid { get; set; }
    }
}

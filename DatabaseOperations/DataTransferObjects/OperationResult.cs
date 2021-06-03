using System.Collections.Generic;

namespace DatabaseOperations.DataTransferObjects
{
    /// <summary>
    /// <para>
    /// The wrapper around the result from the operation that contains the
    /// actual result and type and any messages to feed back to the calling
    /// method.
    /// </para>
    /// <para>
    /// Messages could be errors or information about the process for logging.
    /// </para>
    /// </summary>
    /// <typeparam name="T">
    /// The type to be returned. Normally <see langword="bool" /> .
    /// </typeparam>
    public class OperationResult<T> where T : new()
    {
        /// <summary>
        /// The actual result from the operation.
        /// </summary>
        public T Result { get; set; } = new();

        /// <summary>
        /// The list of messages for the consuming class.
        /// Messages will include errors, validation errors and information messages.
        /// </summary>
        public IList<string> Messages { get; set; } = new List<string>();
    }
}

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
    public class OperationResult
    {
        /// <summary>
        /// The actual result from the operation.
        /// </summary>
        public bool Result { get; set; } = true;

        /// <summary>
        /// The list of messages for the consuming class.
        /// Messages will include errors, validation errors and information messages.
        /// </summary>
        public IList<string> Messages { get; set; } = new List<string>();
    }
}

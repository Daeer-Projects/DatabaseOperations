namespace DatabaseOperations.DataTransferObjects
{
    using Microsoft.Data.SqlClient;

    internal struct BackupProperties
    {
        internal SqlParameter[] BackupParameters { get; set; }
        internal SqlParameter[] ExecutionParameters { get; set; }
        internal string BackupLocation { get; set; }
        internal string BackupPath { get; set; }
        internal string Description { get; set; }
        internal int CommandTimeout { get; set; }
    }
}

namespace DatabaseOperations.DataTransferObjects
{
    using System.IO;
    using Microsoft.Data.SqlClient;

    internal struct BackupProperties
    {
        internal SqlParameter[] BackupParameters { get; set; }
        internal SqlParameter[] ExecutionParameters { get; set; }
        internal string BackupFileName { get; set; }
        internal string BackupPath { get; set; }
        internal string Description { get; set; }
        internal int CommandTimeout { get; set; }

        internal string BackupPathAndFileName()
        {
            return Path.Combine(BackupPath, BackupFileName);
        }
    }
}
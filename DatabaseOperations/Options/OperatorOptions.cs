namespace DatabaseOperations.Options
{
    public class OperatorOptions
    {
        public string BackupPath { get; set; } = string.Empty;
        public int Timeout { get; set; } = 0;

        //public string BackupFileForRestore { get; set; } = string.Empty;
        //public string DatabaseBackupName { get; set; } = string.Empty;
        //public string DatabaseSnapShotName { get; set; } = string.Empty;
        //public bool UseDisk { get; set; } = true;
        //public bool WithRecovery { get; set; } = true;
        //public bool WithReplace { get; set; } = true;
        //public bool WithRestart { get; set; } = false;
        //public bool VerifyOnly { get; set; } = false;
    }
}

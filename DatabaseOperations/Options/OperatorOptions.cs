namespace DatabaseOperations.Options
{
    public sealed class OperatorOptions
    {
        public string BackupPath { get; set; } = string.Empty;
        public int Timeout { get; set; } = 0;
    }
}

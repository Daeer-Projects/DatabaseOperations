namespace DatabaseOperations.DataTransferObjects
{
    public class ConnectionDetails
	{
        public string ServerName { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;
        public string ConnectionString { get; set; } = string.Empty;
        public bool IsValid { get; set; }
	}
}

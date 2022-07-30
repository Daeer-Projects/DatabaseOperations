namespace DatabaseOperations.DataTransferObjects
{
    public class ConnectionProperties
    {
        internal string ConnectionString { get; set; } = string.Empty;
        internal string ApplicationName { get; set; } = string.Empty;
        internal string DatabaseName { get; set; } = string.Empty;
        internal string ConnectTimeout { get; set; } = string.Empty;
        internal string IntegratedSecurity { get; set; } = string.Empty;
        internal string Password { get; set; } = string.Empty;
        internal string Server { get; set; } = string.Empty;
        internal string UserId { get; set; } = string.Empty;
    }
}

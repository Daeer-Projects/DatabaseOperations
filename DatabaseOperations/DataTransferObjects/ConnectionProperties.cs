namespace DatabaseOperations.DataTransferObjects
{
    public struct ConnectionProperties
    {
        internal string ConnectionString { get; set; }
        internal string ApplicationName { get; set; }
        internal string DatabaseName { get; set; }
        internal string ConnectTimeout { get; set; }
        internal string IntegratedSecurity { get; set; }
        internal string Password { get; set; }
        internal string Server { get; set; }
        internal string UserId { get; set; }
    }
}

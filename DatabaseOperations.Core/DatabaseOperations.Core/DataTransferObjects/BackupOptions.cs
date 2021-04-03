using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DatabaseOperations.Core.Tests")]
namespace DatabaseOperations.Core.DataTransferObjects
{
    internal class BackupOptions
    {
        public string DatabaseName { get; set; } = string.Empty;
        public string ConnectionString { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
    }
}

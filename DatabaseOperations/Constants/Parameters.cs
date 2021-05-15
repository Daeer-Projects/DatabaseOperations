using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DatabaseOperations.Tests")]
namespace DatabaseOperations.Constants
{
    internal static class Parameters
    {
        internal const string NameParameter = "@DatabaseName";

        internal const string LocationParameter = "@BackupLocation";

        internal const string DescriptionParameter = "@BackupDescription";

        internal const string PathParameter = "@BackupPath";
    }
}

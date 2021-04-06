using DatabaseOperations.DataTransferObjects;
using DatabaseOperations.Interfaces;
using DatabaseOperations.Validators;
using Useful.Extensions;

namespace DatabaseOperations.Extensions
{
    public static class ConnectionStringExtensions
    {
        public static ConnectionDetails ToConnectionDetails(this string connectionString, IConsoleWrapper console)
        {
            var itemArray = connectionString.Split(';');

            var isValid = itemArray.CheckValidation(new ConnectionArrayValidator());

            if (!isValid.IsValid)
            {
                var validationErrors = string.Join(", ", isValid.Errors);
                console.WriteLine($"Connection String failed validation: {validationErrors}");
                return new ConnectionDetails();
            }

            var server = itemArray[0].SubstringAfterValue("server=");
            var database = itemArray[1].SubstringAfterValue("Database=");

            var details = new ConnectionDetails
            {
                ServerName = server,
                DatabaseName = database,
                ConnectionString = connectionString,
                IsValid = true
            };

            return details;
        }
	}
}

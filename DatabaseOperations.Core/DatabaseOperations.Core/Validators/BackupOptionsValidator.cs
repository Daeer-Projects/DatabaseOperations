using DatabaseOperations.Core.DataTransferObjects;
using FluentValidation;

namespace DatabaseOperations.Core.Validators
{
    internal class BackupOptionsValidator : AbstractValidator<BackupOptions>
    {
        public BackupOptionsValidator()
        {
            // ToDo: Add length checks to these properties.
            RuleFor(options => options).NotNull();
            RuleFor(options => options.ConnectionString).NotEmpty();
            RuleFor(options => options.DatabaseName).NotEmpty();
            RuleFor(options => options.Destination).NotEmpty();
        }
    }
}

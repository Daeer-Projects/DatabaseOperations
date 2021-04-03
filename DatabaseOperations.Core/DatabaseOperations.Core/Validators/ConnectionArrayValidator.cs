using System;
using System.Linq;
using FluentValidation;

namespace DatabaseOperations.Core.Validators
{
    internal class ConnectionArrayValidator : AbstractValidator<string[]>
    {
        public ConnectionArrayValidator()
        {
            RuleFor(data => data).NotNull();
            RuleFor(data => data).NotEmpty();
            RuleFor(data => data.Length).GreaterThan(2);
            RuleFor(data => data).Must(FirstItemIsServer);
            RuleFor(data => data).Must(SecondItemIsDatabase);
        }

        private const string ServerItem = "server=";
        private const string DatabaseItem = "database=";

        private static bool FirstItemIsServer(string[] data)
        {
            bool result;
            try
            {
                result = data.First().ToLower().Contains(ServerItem);
            }
            catch (NullReferenceException)
            {
                result = false;
            }
            catch (InvalidOperationException)
            {
                result = false;
            }
            catch (IndexOutOfRangeException)
            {
                result = false;
            }

            return result;
        }

        private static bool SecondItemIsDatabase(string[] data)
        {
            bool result;
            try
            {
                result = data[1].ToLower().Contains(DatabaseItem);
            }
            catch (NullReferenceException)
            {
                result = false;
            }
            catch (InvalidOperationException)
            {
                result = false;
            }
            catch (IndexOutOfRangeException)
            {
                result = false;
            }

            return result;
        }
	}
}

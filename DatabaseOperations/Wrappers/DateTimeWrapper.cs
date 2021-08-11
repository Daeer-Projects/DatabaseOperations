using System;
using DatabaseOperations.Interfaces;

namespace DatabaseOperations.Wrappers
{
    internal class DateTimeWrapper : IDateTimeWrapper
    {
        internal DateTimeWrapper()
        {
            _dateTime = null;
        }

        internal DateTimeWrapper(DateTime fixedDateTime)
        {
            _dateTime = fixedDateTime;
        }

        private readonly DateTime? _dateTime;

        public DateTime Now => _dateTime ?? DateTime.Now;
    }
}

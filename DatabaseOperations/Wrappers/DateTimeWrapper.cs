namespace DatabaseOperations.Wrappers
{
    using System;
    using Interfaces;

    internal class DateTimeWrapper : IDateTimeWrapper
    {
        internal DateTimeWrapper()
        {
            _dateTime = null;
        }

        private readonly DateTime? _dateTime;

        public DateTime Now => _dateTime ?? DateTime.Now;
    }
}
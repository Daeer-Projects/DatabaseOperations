namespace DatabaseOperations.Wrappers
{
    using System;
    using Interfaces;

    internal class DateTimeWrapper : IDateTimeWrapper
    {
        internal DateTimeWrapper()
        {
            dateTime = null;
        }

        private readonly DateTime? dateTime;

        public DateTime Now => dateTime ?? DateTime.Now;
    }
}
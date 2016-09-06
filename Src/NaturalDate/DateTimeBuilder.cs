using System;

namespace NaturalDate
{
    internal sealed class DateTimeBuilder : IDateTimeBuilder<DateTime>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="reference">The initial reference date.</param>
        internal DateTimeBuilder(DateTime reference)
        {
            Reference = reference;

            Day = Reference.Day;
            Month = Reference.Month;
            Year = Reference.Year;
            Hour = Reference.Hour;
            Minute = Reference.Minute;
            Second = Reference.Second;
        }

        /// <summary>
        /// Returns a new date builder initialized to the current time.
        /// </summary>
        /// <returns>The date builder that was created and initialized to the current time.</returns>
        public static IDateTimeBuilder<DateTime> Now()
        {
            return new DateTimeBuilder(DateTime.Now);
        }

        /// <summary>
        /// Returns the date that has been built.
        /// </summary>
        /// <returns>The date that has been built.</returns>
        public DateTime Build()
        {
            return new DateTime(Year, Month, Day, Hour, Minute, Second);
        }

        /// <summary>
        /// Gets the reference date.
        /// </summary>
        internal DateTime Reference { get; set; }

        /// <summary>
        /// Gets or sets the day portion of the date.
        /// </summary>
        public int Day { get; set; }

        /// <summary>
        /// Gets or sets the month portion of the date.
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        /// Gets or sets the year portion of the date.
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Gets or sets the hour portion of the time.
        /// </summary>
        public int Hour { get; set; }

        /// <summary>
        /// Gets or sets the minutes portion of the time.
        /// </summary>
        public int Minute { get; set; }

        /// <summary>
        /// Gets or sets the seconds portion of the time.
        /// </summary>
        public int Second { get; set; }
    }
}
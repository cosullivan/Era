using System;

namespace NaturalDate
{
    public sealed class DateBuilder : IDateBuilder<DateTime>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="reference">The initial reference date.</param>
        internal DateBuilder(DateTime reference)
        {
            Reference = reference;

            Day = Reference.Day;
            Month = Reference.Month;
            Year = Reference.Year;
        }

        /// <summary>
        /// Returns a new date builder initialized to the current time.
        /// </summary>
        /// <returns>The date builder that was created and initialized to the current time.</returns>
        public static IDateBuilder<DateTime> Now()
        {
            return new DateBuilder(DateTime.Now);
        }

        /// <summary>
        /// Returns the date that has been built.
        /// </summary>
        /// <returns>The date that has been built.</returns>
        public DateTime Build()
        {
            return new DateTime(Year, Month, Day, 0, 0, 0);
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
    }
}
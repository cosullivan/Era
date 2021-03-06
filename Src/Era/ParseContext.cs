﻿namespace Era
{
    internal sealed class ParseContext
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        internal ParseContext() { }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="builder">The date/time builder to initialize the context with.</param>
        internal ParseContext(IDateTimeBuilder builder)
        {
            Year = builder.Year;
            Month = builder.Month;
            Day = builder.Day;
            Hour = builder.Hour;
            Minute = builder.Minute;
            Second = builder.Second;
        }

        /// <summary>
        /// Gets or sets the day portion of the date.
        /// </summary>
        internal int Day { get; set; }

        /// <summary>
        /// Gets or sets the month portion of the date.
        /// </summary>
        internal int Month { get; set; }

        /// <summary>
        /// Gets or sets the year portion of the date.
        /// </summary>
        internal int Year { get; set; }

        /// <summary>
        /// Gets or sets the hour portion of the time.
        /// </summary>
        internal int Hour { get; set; }

        /// <summary>
        /// Gets or sets the minutes portion of the time.
        /// </summary>
        internal int Minute { get; set; }

        /// <summary>
        /// Gets or sets the seconds portion of the time.
        /// </summary>
        internal int Second { get; set; }
    }
}
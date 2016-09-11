namespace NaturalDate
{
    internal sealed class ParseContext : IDateTimeBuilder
    {
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
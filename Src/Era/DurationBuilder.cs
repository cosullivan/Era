using System;

namespace Era
{
    internal sealed class DurationBuilder : IDurationBuilder<TimeSpan>
    {
        readonly DateTime _reference;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="reference">the reference time to add the duration to.</param>
        internal DurationBuilder(DateTime reference)
        {
            _reference = reference;
        }

        /// <summary>
        /// Returns the time span has been built.
        /// </summary>
        /// <returns>The time span that has been built.</returns>
        public TimeSpan Build()
        {
            var time = _reference;

            time = AddYears(time, Year);
            time = AddMonths(time, Month);
            time = time.AddDays(Decimal.ToDouble(Day));
            time = time.AddHours(Decimal.ToDouble(Hour));
            time = time.AddMinutes(Decimal.ToDouble(Minute));
            time = time.AddSeconds(Decimal.ToDouble(Second));

            return time - _reference;
        }

        /// <summary>
        /// Adds the decimal years to the time.
        /// </summary>
        /// <param name="time">The time to add the decimal years to.</param>
        /// <param name="duration">The years duration to add to the time.</param>
        /// <returns>The time which has the decimal years added to it.</returns>
        DateTime AddYears(DateTime time, decimal duration)
        {
            while (duration >= 1)
            {
                time = time.AddYears(1);

                duration -= 1;
            }

            return AddMonths(time, duration * 12);
        }

        /// <summary>
        /// Adds the decimal months to the time.
        /// </summary>
        /// <param name="time">The time to add the decimal months to.</param>
        /// <param name="duration">The months duration to add to the time.</param>
        /// <returns>The time which has the decimal months added to it.</returns>
        DateTime AddMonths(DateTime time, decimal duration)
        {
            while (duration >= 1)
            {
                time = time.AddMonths(1);

                duration -= 1;
            }

            return time.AddDays((double)(duration * DateTime.DaysInMonth(time.Year, time.Month)));
        }

        /// <summary>
        /// Gets or sets the decimal years of the time span.
        /// </summary>
        public decimal Year { get; set; }

        /// <summary>
        /// Gets or sets the decimal months of the time span.
        /// </summary>
        public decimal Month { get; set; }

        /// <summary>
        /// Gets or sets the decimal days of the time span.
        /// </summary>
        public decimal Day { get; set; }

        /// <summary>
        /// Gets or sets the decimal hours of the time span.
        /// </summary>
        public decimal Hour { get; set; }

        /// <summary>
        /// Gets or sets the decimal minutes of the time span.
        /// </summary>
        public decimal Minute { get; set; }

        /// <summary>
        /// Gets or sets the decimal seconds of the time span.
        /// </summary>
        public decimal Second { get; set; }
    }
}
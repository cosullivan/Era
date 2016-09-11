using System;

namespace NaturalDate
{
    public interface IDateTimeBuilder<out T> : IDateTimeBuilder
    {
        /// <summary>
        /// Returns the date that has been built.
        /// </summary>
        /// <returns>The date that has been built.</returns>
        T Build();
    }

    public interface IDateTimeBuilder
    {
        /// <summary>
        /// Gets or sets the day portion of the date.
        /// </summary>
        int Day { get; set; }

        /// <summary>
        /// Gets or sets the month portion of the date.
        /// </summary>
        int Month { get; set; }

        /// <summary>
        /// Gets or sets the year portion of the date.
        /// </summary>
        int Year { get; set; }

        /// <summary>
        /// Gets or sets the hour portion of the time.
        /// </summary>
        int Hour { get; set; }

        /// <summary>
        /// Gets or sets the minutes portion of the time.
        /// </summary>
        int Minute { get; set; }

        /// <summary>
        /// Gets or sets the seconds portion of the time.
        /// </summary>
        int Second { get; set; }
    }

    internal static class DateTimeBuilderExtensions
    {
        /// <summary>
        /// Returns the builder as a CLR DateTime instance.
        /// </summary>
        /// <param name="builder">The builder to return as a CLR DateTime instance.</param>
        /// <returns>The CLR DateTime instance that represents the current state of the builder.</returns>
        internal static DateTime DateTime(this IDateTimeBuilder builder)
        {
            if (builder == null)
            { 
                throw new ArgumentNullException(nameof(builder));
            }

            return new DateTime(builder.Year, builder.Month, builder.Day, builder.Hour, builder.Minute, builder.Second);
        }
    }
}
namespace Era
{
    public interface IDurationBuilder<out T> : IDurationBuilder
    {
        /// <summary>
        /// Returns the time span has been built.
        /// </summary>
        /// <returns>The time span that has been built.</returns>
        T Build();
    }

    public interface IDurationBuilder
    {
        /// <summary>
        /// Gets or sets the decimal years of the time span.
        /// </summary>
        decimal Year { get; set; }

        /// <summary>
        /// Gets or sets the decimal months of the time span.
        /// </summary>
        decimal Month { get; set; }

        /// <summary>
        /// Gets or sets the decimal days of the time span.
        /// </summary>
        decimal Day { get; set; }

        /// <summary>
        /// Gets or sets the decimal hours of the time span.
        /// </summary>
        decimal Hour { get; set; }

        /// <summary>
        /// Gets or sets the decimal minutes of the time span.
        /// </summary>
        decimal Minute { get; set; }

        /// <summary>
        /// Gets or sets the decimal seconds of the time span.
        /// </summary>
        decimal Second { get; set; }
    }
}
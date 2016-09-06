namespace NaturalDate
{
    public interface ITimeBuilder
    {
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
}
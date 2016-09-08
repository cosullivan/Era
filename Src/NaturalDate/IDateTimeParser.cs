namespace NaturalDate
{
    public interface IDateTimeParser
    {
        /// <summary>
        /// Attempt to make a date & time.
        /// </summary>
        /// <param name="builder">The date builder that was used to build the date & time.</param>
        /// <returns>true if a date & time could be made, false if not.</returns>
        bool TryMake(IDateTimeBuilder builder);
    }
}

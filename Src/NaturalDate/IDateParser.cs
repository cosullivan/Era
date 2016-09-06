namespace NaturalDate
{
    public interface IDateParser
    {
        /// <summary>
        /// Attempt to make a date.
        /// </summary>
        /// <param name="builder">The date builder that was used to build the date.</param>
        /// <returns>true if a date could be made, false if not.</returns>
        bool TryMakeDate(IDateBuilder builder);
    }
}

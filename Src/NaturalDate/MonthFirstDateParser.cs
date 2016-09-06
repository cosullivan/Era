using NaturalDate.Text;

namespace NaturalDate
{
    internal sealed class MonthFirstDateParser : CalendarDateParser
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="enumerator">The token enumerator to handle the incoming tokens.</param>
        internal MonthFirstDateParser(TokenEnumerator enumerator) : base(enumerator) { }

        /// <summary>
        /// Attempt to make a date.
        /// </summary>
        /// <param name="builder">The date builder that was used to build the date.</param>
        /// <returns>true if a date could be made, false if not.</returns>
        public override bool TryMakeDate(IDateBuilder builder)
        {
            builder.Day = 1;

            int month;
            if (TryMakeMonthPartText(out month) == false)
            {
                return false;
            }

            if (Enumerator.Peek().Kind == TokenKind.None)
            {
                builder.Month = month;
                return true;
            }

            if (TryMakeSeparator() == false)
            {
                return false;
            }

            int year;
            if (TryMakeYearPart(out year))
            {
                builder.Year = year;
                return true;
            }

            return false;
        }
    }
}
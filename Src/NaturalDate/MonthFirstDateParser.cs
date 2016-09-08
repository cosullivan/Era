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
        /// Attempt to make a date & time.
        /// </summary>
        /// <param name="builder">The date builder that was used to build the date & time.</param>
        /// <returns>true if a date & time could be made, false if not.</returns>
        public override bool TryMake(IDateTimeBuilder builder)
        {
            int month;
            if (TryMakeMonthPartText(out month) == false)
            {
                return false;
            }

            if (Enumerator.Peek().Kind == TokenKind.None)
            {
                return TryMake(builder, builder.Year, month, 1);
            }

            if (TryMakeSeparator() == false)
            {
                return false;
            }

            int year;
            if (TryMakeYearPart(out year))
            {
                return TryMake(builder, year, month, 1);
            }

            return false;
        }
    }
}
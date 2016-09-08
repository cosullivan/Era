using System;
using NaturalDate.Text;

namespace NaturalDate
{
    internal sealed class YearFirstDateParser : CalendarDateParser
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="enumerator">The token enumerator to handle the incoming tokens.</param>
        internal YearFirstDateParser(TokenEnumerator enumerator) : base(enumerator) { }

        /// <summary>
        /// Attempt to make a date & time.
        /// </summary>
        /// <param name="builder">The date builder that was used to build the date & time.</param>
        /// <returns>true if a date & time could be made, false if not.</returns>
        public override bool TryMake(IDateTimeBuilder builder)
        {
            int year;
            if (TryMake4DigitYearPart(out year) == false)
            {
                return false;
            }

            if (Enumerator.Peek().Kind == TokenKind.None)
            {
                return TryMake(builder, year, 1, 1);
            }

            if (TryMakeSeparator() == false)
            {
                return false;
            }

            int month;
            if (TryMakeMonthPart(out month) == false)
            {
                return false;
            }

            if (Enumerator.Peek().Kind == TokenKind.None)
            {
                return TryMake(builder, year, month, 1);
            }

            if (TryMakeSeparator() == false)
            {
                return false;
            }

            int day;
            if (TryMakeDayPart(out day) && day < DateTime.DaysInMonth(builder.Year, builder.Month))
            {
                return TryMake(builder, year, month, day);
            }

            return false;
        }
    }
}
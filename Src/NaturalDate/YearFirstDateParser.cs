using System;
using NaturalDate.Text;

namespace NaturalDate
{
    internal sealed class YearFirstDateParser : DateParser
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="enumerator">The token enumerator to handle the incoming tokens.</param>
        internal YearFirstDateParser(TokenEnumerator enumerator) : base(enumerator) { }

        /// <summary>
        /// Attempt to make a date.
        /// </summary>
        /// <param name="builder">The date builder that was used to build the date.</param>
        /// <returns>true if a date could be made, false if not.</returns>
        public override bool TryMakeDate(IDateBuilder builder)
        {
            int year;
            if (TryMake4DigitYearPart(out year) == false)
            {
                return false;
            }

            builder.Year = year;

            if (Enumerator.Peek().Kind == TokenKind.None)
            {
                builder.Month = 1;
                builder.Day = 1;
                return true;
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

            builder.Month = month;

            if (Enumerator.Peek().Kind == TokenKind.None)
            {
                builder.Day = 1;
                return true;
            }

            if (TryMakeSeparator() == false)
            {
                return false;
            }

            int day;
            if (TryMakeDayPart(out day) && day < DateTime.DaysInMonth(builder.Year, builder.Month))
            {
                builder.Day = day;
                return true;
            }

            return false;
        }
    }
}
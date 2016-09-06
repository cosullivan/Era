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
        /// Attempt to make a date.
        /// </summary>
        /// <param name="builder">The date builder that was used to build the date.</param>
        /// <returns>true if a date could be made, false if not.</returns>
        public override bool TryMakeDate(IDateTimeBuilder builder)
        {
            int year;
            if (TryMake4DigitYearPart(out year) == false)
            {
                return false;
            }

            if (Enumerator.Peek().Kind == TokenKind.None)
            {
                builder.Year = year;
                builder.Month = 1;
                builder.Day = 1;

                return base.TryMakeDate(builder);
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
                builder.Year = year;
                builder.Month = month;
                builder.Day = 1;

                return base.TryMakeDate(builder);
            }

            if (TryMakeSeparator() == false)
            {
                return false;
            }

            int day;
            if (TryMakeDayPart(out day) && day < DateTime.DaysInMonth(builder.Year, builder.Month))
            {
                builder.Year = year;
                builder.Month = month;
                builder.Day = day;

                return base.TryMakeDate(builder);
            }

            return false;
        }
    }
}
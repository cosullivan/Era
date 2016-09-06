using System;
using NaturalDate.Text;

namespace NaturalDate
{
    internal sealed class DayFirstDateParser : CalendarDateParser
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="enumerator">The token enumerator to handle the incoming tokens.</param>
        internal DayFirstDateParser(TokenEnumerator enumerator) : base(enumerator) { }

        /// <summary>
        /// Attempt to make a date.
        /// </summary>
        /// <param name="builder">The date builder that was used to build the date.</param>
        /// <returns>true if a date could be made, false if not.</returns>
        public override bool TryMakeDate(IDateBuilder builder)
        {
            int day;
            if (TryMakeDayPart(out day) == false)
            {
                return false;
            }

            if (Enumerator.Peek().Kind == TokenKind.None)
            {
                if (day < DateTime.DaysInMonth(builder.Year, builder.Month))
                {
                    builder.Day = day;
                    return true;
                }

                return false;
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
                builder.Month = month;

                if (day < DateTime.DaysInMonth(builder.Year, builder.Month))
                {
                    builder.Day = day;
                    return true;
                }

                return false;
            }

            if (TryMakeSeparator() == false)
            {
                return false;
            }

            int year;
            if (TryMakeYearPart(out year) == false)
            {
                return false;
            }

            builder.Year = year < 100 ? 2000 + year : year;
            builder.Month = month;

            if (day < DateTime.DaysInMonth(builder.Year, builder.Month))
            {
                builder.Day = day;
                return true;
            }

            return false;
        }
    }
}
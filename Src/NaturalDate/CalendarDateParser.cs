using System;
using System.Collections.Generic;
using NaturalDate.Text;

namespace NaturalDate
{
    internal sealed class CalendarDateParser : Parser
    {
        static readonly Token[] DateSeparators = new Token[]
        {
            new Token(TokenKind.Space, ' '),
            new Token(TokenKind.Punctuation, '/'),
            new Token(TokenKind.Punctuation, '-'),
            new Token(TokenKind.Punctuation, '.')
        };

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="enumerator">The token enumerator to handle the incoming tokens.</param>
        internal CalendarDateParser(TokenEnumerator enumerator) : base(enumerator) { }

        /// <summary>
        /// Attempt to make a date.
        /// </summary>
        /// <param name="builder">The date builder that was used to build the date.</param>
        /// <returns>true if a date could be made, false if not.</returns>
        public bool TryMakeDate(IDateBuilder builder)
        {
            int day;
            if (TryMakeDayPart(out day))
            {
                return TryMakeDateWithDay(builder, day);
            }

            int year;
            if (TryMake4DigitYearPart(out year))
            {
                return TryMakeDateWithYear(builder, year);
            }

            return false;
        }

        /// <summary>
        /// Attempt to make a day first date.
        /// </summary>
        /// <param name="builder">The date builder to populate from.</param>
        /// <param name="day">The day that is to start the parsing.</param>
        /// <returns>true if a day first date could be made, false if not.</returns>
        bool TryMakeDateWithDay(IDateBuilder builder, int day)
        {
            if (Enumerator.Peek().Kind == TokenKind.None)
            {
                if (day < DateTime.DaysInMonth(builder.Year, builder.Month))
                {
                    builder.Day = day;
                    return true;
                }

                return false;
            }

            Token separator;
            if (TryMakeToken(DateSeparators, out separator) == false)
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

            if (TryMakeToken(separator) == false)
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

        /// <summary>
        /// Attempt to make a date starting with a month.
        /// </summary>
        /// <param name="builder">The date builder to populate from.</param>
        /// <returns>true if a month first date could be made, false if not.</returns>
        bool TryMakeDateWithMonth(IDateBuilder builder)
        {
            return false;
        }

        /// <summary>
        /// Attempt to make a year first date.
        /// </summary>
        /// <param name="builder">The date builder to populate from.</param>
        /// <param name="year">The year that is to start the parsing.</param>
        /// <returns>true if a year first date could be made, false if not.</returns>
        bool TryMakeDateWithYear(IDateBuilder builder, int year)
        {
            builder.Year = year;

            if (Enumerator.Peek().Kind == TokenKind.None)
            {
                builder.Month = 1;
                builder.Day = 1;
                return true;
            }

            Token separator;
            if (TryMakeToken(DateSeparators, out separator) == false)
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

            if (TryMakeToken(separator) == false)
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

        /// <summary>
        /// Attempt to make the day part of the date.
        /// </summary>
        /// <param name="day">The day part of a date.</param>
        /// <returns>true if a day part could be made, false if not.</returns>
        bool TryMakeDayPart(out int day)
        {
            return TryMakeNumeric(1, 2, out day) && day > 0 && day <= 31; 
        }

        /// <summary>
        /// Attempt to make the month part of the date.
        /// </summary>
        /// <param name="month">The month part of a date.</param>
        /// <returns>true if a month part could be made, false if not.</returns>
        bool TryMakeMonthPart(out int month)
        {
            if (TryMakeMonthPartNumeric(out month))
            {
                return true;
            }

            return TryMakeMonthPartText(out month);
        }

        /// <summary>
        /// Attempt to make the month part of the date.
        /// </summary>
        /// <param name="month">The month part of a date.</param>
        /// <returns>true if a month part could be made, false if not.</returns>
        bool TryMakeMonthPartNumeric(out int month)
        {
            return TryMakeNumeric(1, 2, out month) && month > 0 && month <= 12;
        }

        /// <summary>
        /// Attempt to make the month part of the date.
        /// </summary>
        /// <param name="month">The month part of a date.</param>
        /// <returns>true if a month part could be made, false if not.</returns>
        bool TryMakeMonthPartText(out int month)
        {
            month = 0;

            var token = Enumerator.Peek();
            if (token.Kind != TokenKind.Text)
            {
                return false;
            }

            // TODO: pull this from a lanugage translation service somewhere
            var dictionary = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
            {
                { "Jan", 1 },
                { "Feb", 2 },
                { "Mar", 3 },
                { "Apr", 4 },
                { "May", 5 },
                { "Jun", 6 },
                { "Jul", 7 },
                { "Aug", 8 },
                { "Sep", 9 },
                { "Oct", 10 },
                { "Nov", 11 },
                { "Dec", 12 }
            };

            if (dictionary.TryGetValue(token.Text, out month))
            {
                Enumerator.Take();
                return month > 0 && month <= 12;
            }

            return false;
        }

        /// <summary>
        /// Attempt to make the year part of the date.
        /// </summary>
        /// <param name="year">The year part of a date.</param>
        /// <returns>true if a year part could be made, false if not.</returns>
        bool TryMakeYearPart(out int year)
        {
            return TryMake2DigitYearPart(out year) || TryMake4DigitYearPart(out year);
        }

        /// <summary>
        /// Attempt to make the year part of the date.
        /// </summary>
        /// <param name="year">The year part of a date.</param>
        /// <returns>true if a year part could be made, false if not.</returns>
        bool TryMake2DigitYearPart(out int year)
        {
            return TryMakeNumeric(2, out year) && year >= 0 && year <= 9999;
        }

        /// <summary>
        /// Attempt to make the year part of the date.
        /// </summary>
        /// <param name="year">The year part of a date.</param>
        /// <returns>true if a year part could be made, false if not.</returns>
        bool TryMake4DigitYearPart(out int year)
        {
            return TryMakeNumeric(4, out year) && year >= 0 && year <= 9999;
        }
    }
}
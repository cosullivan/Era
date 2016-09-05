using System;
using System.Collections.Generic;
using NaturalDate.Text;

namespace NaturalDate
{
    internal sealed class CalendarDateParser : Parser
    {
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
            if (TryMake(TryMakeDayPart, out day) == false)
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

            Token original;
            if (TryMakeDateSeparator(out original) == false)
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

            Token separator;
            if (TryMakeDateSeparator(out separator) == false || separator != original)
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
        /// Attempt to make the day part of the date.
        /// </summary>
        /// <param name="day">The day part of a date.</param>
        /// <returns>true if a day part could be made, false if not.</returns>
        internal bool TryMakeDayPart(out int day)
        {
            day = 0;

            var token = Enumerator.Take();
            if (token.Kind != TokenKind.Number || token.LeadingZeros() > 1)
            {
                return false;
            }

            day = token.AsInteger();

            return day > 0 && day <= 31; 
        }

        /// <summary>
        /// Attempt to make the month part of the date.
        /// </summary>
        /// <param name="month">The month part of a date.</param>
        /// <returns>true if a month part could be made, false if not.</returns>
        internal bool TryMakeMonthPart(out int month)
        {
            if (TryMake(TryMakeMonthPartNumeric, out month))
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
        internal bool TryMakeMonthPartNumeric(out int month)
        {
            month = 0;

            var token = Enumerator.Take();
            if (token.Kind != TokenKind.Number || token.LeadingZeros() > 1)
            {
                return false;
            }

            month = token.AsInteger();

            return month > 0 && month <= 12;
        }

        /// <summary>
        /// Attempt to make the month part of the date.
        /// </summary>
        /// <param name="month">The month part of a date.</param>
        /// <returns>true if a month part could be made, false if not.</returns>
        internal bool TryMakeMonthPartText(out int month)
        {
            month = 0;

            var token = Enumerator.Take();
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
                return month > 0 && month <= 12;
            }

            return false;
        }

        /// <summary>
        /// Attempt to make the year part of the date.
        /// </summary>
        /// <param name="year">The year part of a date.</param>
        /// <returns>true if a year part could be made, false if not.</returns>
        internal bool TryMakeYearPart(out int year)
        {
            year = 0;

            var token = Enumerator.Take();
            if (token.Kind != TokenKind.Number || !(token.Text.Length == 2 || token.Text.Length == 4))
            {
                return false;
            }

            year = token.AsInteger();

            return year >= 0 && year <= 9999;
        }

        /// <summary>
        /// Attempt to make a date separator.
        /// </summary>
        /// <param name="token">The token that represents the separator.</param>
        /// <returns>true if a month part could be made, false if not.</returns>
        internal bool TryMakeDateSeparator(out Token token)
        {
            token = Enumerator.Take();

            return token == new Token(TokenKind.Space, ' ')
                || token == new Token(TokenKind.Punctuation, '/')
                || token == new Token(TokenKind.Punctuation, '-') 
                || token == new Token(TokenKind.Punctuation, '.');
        }
    }
}
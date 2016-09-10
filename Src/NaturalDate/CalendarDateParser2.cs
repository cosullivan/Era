using System;
using System.Collections.Generic;
using NaturalDate.Text;

namespace NaturalDate
{
    internal sealed class CalendarDateParser2 : TokenParser, IDateTimeParser
    {
        static readonly Token[] DateSeparators =
        {
            new Token(TokenKind.Space, ' '),
            new Token(TokenKind.Punctuation, '/'),
            new Token(TokenKind.Punctuation, '-'),
            new Token(TokenKind.Punctuation, '.')
        };

        Token _separator = Token.None;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="enumerator">The token enumerator to handle the incoming tokens.</param>
        internal CalendarDateParser2(TokenEnumerator enumerator) : base(enumerator) { }

        /// <summary>
        /// Attempt to make a date & time.
        /// </summary>
        /// <param name="builder">The date builder that was used to build the date & time.</param>
        /// <returns>true if a date & time could be made, false if not.</returns>
        public bool TryMake(IDateTimeBuilder builder)
        {
            return TryMakeDate(builder);
        }

        /// <summary>
        /// Try to make the date with the given inputs.
        /// </summary>
        /// <param name="builder">The builder to build the date from.</param>
        /// <param name="year">The year to build the date from.</param>
        /// <param name="month">The month to build the date from.</param>
        /// <param name="day">The day to build the date from.</param>
        /// <returns>true if the date could be made, false if not.</returns>
        static bool TryAccept(IDateTimeBuilder builder, int year, int month, int day)
        {
            if (day > DateTime.DaysInMonth(year, month))
            {
                return false;
            }

            builder.Day = day;
            builder.Month = month;
            builder.Year = year < 100 ? 2000 + year : year;

            return true;
        }

        /// <summary>
        /// Try to make a date.
        /// </summary>
        /// <param name="builder">The builder to make the date from.</param>
        /// <returns>true if the date could be made, false if not.</returns>
        bool TryMakeDate(IDateTimeBuilder builder)
        {
            return TryMake(TryMakeDayMonthYear, builder)
                || TryMake(TryMakeDayMonth, builder)
                || TryMake(TryMakeMonthYear, builder)
                || TryMake(TryMakeMonth, builder)
                || TryMake(TryMakeYearMonthDay, builder)
                || TryMake(TryMakeYear, builder)
                || TryMake(TryMakeDay, builder);
        }

        /// <summary>
        /// Try to make a day, month and year date.
        /// </summary>
        /// <param name="builder">The builder to build from.</param>
        /// <returns>true if the day, month and year date could be made, false if not.</returns>
        bool TryMakeDayMonthYear(IDateTimeBuilder builder)
        {
            int day;
            if (TryMake(TryMakeDayPart, out day) == false)
            {
                return false;
            }

            Token separator;
            if (TryMakeToken(DateSeparators, out separator) == false)
            {
                return false;
            }

            int month;
            if (TryMake(TryMakeMonthPart, out month) == false)
            {
                return false;
            }

            if (TryMakeToken(separator) == false)
            {
                return false;
            }

            int year;
            if (TryMake(TryMakeYearPart, out year) == false)
            {
                return false;
            }

            return TryAccept(builder, year, month, day);
        }

        /// <summary>
        /// Try to make a day and month.
        /// </summary>
        /// <param name="builder">The builder to build from.</param>
        /// <returns>true if the day and month only date could be made, false if not.</returns>
        bool TryMakeDayMonth(IDateTimeBuilder builder)
        {
            int day;
            if (TryMake(TryMakeDayPart, out day) == false)
            {
                return false;
            }

            Token separator;
            if (TryMakeToken(DateSeparators, out separator) == false)
            {
                return false;
            }

            int month;
            if (TryMake(TryMakeMonthPart, out month) == false)
            {
                return false;
            }

            return TryAccept(builder, builder.Year, month, day);
        }

        /// <summary>
        /// Try to make a single day.
        /// </summary>
        /// <param name="builder">The builder to build from.</param>
        /// <returns>true if the day only date could be made, false if not.</returns>
        bool TryMakeDay(IDateTimeBuilder builder)
        {
            int day;
            if (TryMake(TryMakeDayPart, out day) == false)
            {
                return false;
            }

            return TryAccept(builder, builder.Year, builder.Month, day);
        }

        /// <summary>
        /// Try to make a month & year date.
        /// </summary>
        /// <param name="builder">The builder to build the date from.</param>
        /// <returns>true if a month & year could be made, false if not.</returns>
        bool TryMakeMonthYear(IDateTimeBuilder builder)
        {
            return TryMake(TryMakeMonthNameYear, builder) || TryMake(TryMakeMonth4DigitYear, builder);
        }

        /// <summary>
        /// Try to make a month name & year date.
        /// </summary>
        /// <param name="builder">The builder to build the date from.</param>
        /// <returns>true if a month name & year could be made, false if not.</returns>
        bool TryMakeMonthNameYear(IDateTimeBuilder builder)
        {
            int month;
            if (TryMake(TryMakeMonthName, out month) == false)
            {
                return false;
            }

            if (TryMakeToken(DateSeparators) == false)
            {
                return false;
            }

            int year;
            if (TryMake(TryMakeYearPart, out year) == false)
            {
                return false;
            }

            return TryAccept(builder, year, month, 1);
        }

        /// <summary>
        /// Try to make a month digit & 4 digit year date.
        /// </summary>
        /// <param name="builder">The builder to build the date from.</param>
        /// <returns>true if a month name & 4 digit year could be made, false if not.</returns>
        bool TryMakeMonth4DigitYear(IDateTimeBuilder builder)
        {
            int month;
            if (TryMake(TryMakeMonthNumeric, out month) == false)
            {
                return false;
            }

            if (TryMakeToken(DateSeparators) == false)
            {
                return false;
            }

            int year;
            if (TryMake(TryMake4DigitYear, out year) == false)
            {
                return false;
            }

            return TryAccept(builder, year, month, 1);
        }

        /// <summary>
        /// Try to make a single month.
        /// </summary>
        /// <param name="builder">The builder to build from.</param>
        /// <returns>true if the month only date could be made, false if not.</returns>
        bool TryMakeMonth(IDateTimeBuilder builder)
        {
            int month;
            if (TryMake(TryMakeMonthName, out month) == false)
            {
                return false;
            }

            return TryAccept(builder, builder.Year, month, 1);
        }

        /// <summary>
        /// Try to make a year, month and day.
        /// </summary>
        /// <param name="builder">The date builder to build on.</param>
        /// <returns>true if the year, month and day could be made, false if not.</returns>
        bool TryMakeYearMonthDay(IDateTimeBuilder builder)
        {
            int year;
            if (TryMake4DigitYear(out year) == false)
            {
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

            if (TryMakeToken(separator) == false)
            {
                return false;
            }

            int day;
            if (TryMakeDayPart(out day) == false)
            {
                return false;
            }

            return TryAccept(builder, year, month, day);
        }

        /// <summary>
        /// Try to make a year, month and day.
        /// </summary>
        /// <param name="builder">The date builder to build on.</param>
        /// <returns>true if the year, month and day could be made, false if not.</returns>
        bool TryMakeYearMonth(IDateTimeBuilder builder)
        {
            int year;
            if (TryMake4DigitYear(out year) == false)
            {
                return false;
            }

            if (TryMakeToken(DateSeparators) == false)
            {
                return false;
            }

            int month;
            if (TryMakeMonthPart(out month) == false)
            {
                return false;
            }

            return TryAccept(builder, year, month, 1);
        }

        /// <summary>
        /// Try to make a year, month and day.
        /// </summary>
        /// <param name="builder">The date builder to build on.</param>
        /// <returns>true if the year, month and day could be made, false if not.</returns>
        bool TryMakeYear(IDateTimeBuilder builder)
        {
            int year;
            if (TryMake4DigitYear(out year) == false)
            {
                return false;
            }

            return TryAccept(builder, year, 1, 1);
        }

        /// <summary>
        /// Try to make the day portion of a date.
        /// </summary>
        /// <param name="day">The day portion that was made.</param>
        /// <returns>true if the day portion could be made, false if not.</returns>
        bool TryMakeDayPart(out int day)
        {
            if (TryMake(TryMakeNumber, 2, out day) == false && TryMake(TryMakeNumber, 1, out day) == false)
            {
                return false;
            }

            return day >= 1 && day <= 31;
        }

        /// <summary>
        /// Attempt to make the month part of the date.
        /// </summary>
        /// <param name="month">The month part of a date.</param>
        /// <returns>true if a month part could be made, false if not.</returns>
        bool TryMakeMonthPart(out int month)
        {
            return TryMake(TryMakeMonthNumeric, out month) || TryMake(TryMakeMonthName, out month);
        }

        /// <summary>
        /// Attempt to make the year part of the date.
        /// </summary>
        /// <param name="year">The year part of a date.</param>
        /// <returns>true if a year part could be made, false if not.</returns>
        bool TryMakeYearPart(out int year)
        {
            return TryMake(TryMake4DigitYear, out year) || TryMake(TryMake2DigitYear, out year);
        }

        /// <summary>
        /// Attempt to make a 2 digit year.
        /// </summary>
        /// <param name="year">The 2 digit year that was made.</param>
        /// <returns>true if a year part could be made, false if not.</returns>
        bool TryMake2DigitYear(out int year)
        {
            return TryMake(TryMakeNumber, 2, out year) && year >= 0 && year <= 99;
        }

        /// <summary>
        /// Attempt to make a 4 digit year.
        /// </summary>
        /// <param name="year">The 4 digit year that was made.</param>
        /// <returns>true if a year part could be made, false if not.</returns>
        bool TryMake4DigitYear(out int year)
        {
            return TryMake(TryMakeNumber, 4, out year) && year >= 0 && year <= 9999;
        }

        /// <summary>
        /// Attempt to make the month part of the date.
        /// </summary>
        /// <param name="month">The month part of a date.</param>
        /// <returns>true if a month part could be made, false if not.</returns>
        bool TryMakeMonthNumeric(out int month)
        {
            if (TryMake(TryMakeNumber, 2, out month) == false && TryMake(TryMakeNumber, 1, out month) == false)
            {
                return false;
            }

            return month >= 1 && month <= 12;
        }

        /// <summary>
        /// Attempt to make the month part of the date.
        /// </summary>
        /// <param name="month">The month part of a date.</param>
        /// <returns>true if a month part could be made, false if not.</returns>
        bool TryMakeMonthName(out int month)
        {
            month = 0;

            var token = Enumerator.Peek();
            if (token.Kind != TokenKind.Text)
            {
                return false;
            }

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
                { "Dec", 12 },
                { "January", 1 },
                { "February", 2 },
                { "March", 3 },
                { "April", 4 },
                { "June", 6 },
                { "July", 7 },
                { "August", 8 },
                { "September", 9 },
                { "October", 10 },
                { "November", 11 },
                { "December", 12 }
            };

            if (dictionary.TryGetValue(token.Text, out month))
            {
                Enumerator.Take();
                return month > 0 && month <= 12;
            }

            return false;
        }

        /// <summary>
        /// Try to make a number of a specific maximum digit length.
        /// </summary>
        /// <param name="maxLength">The maximum digit length.</param>
        /// <param name="value">The number that was made.</param>
        /// <returns>true if a number could be made, false if not.</returns>
        bool TryMakeNumber(int maxLength, out int value)
        {
            value = 0;

            while (maxLength > 0 && Enumerator.Peek().Kind == TokenKind.Digit)
            {
                value = (value * 10) + Enumerator.Take().AsInteger();
                maxLength = maxLength - 1;
            }

            return maxLength == 0 && Enumerator.Peek().Kind != TokenKind.Digit;
        }
    }
}
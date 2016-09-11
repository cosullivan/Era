using System;
using System.Collections.Generic;
using NaturalDate.Text;

namespace NaturalDate
{
    internal sealed class CalendarDateParser : TokenParser, IDateTimeParser
    {
        static readonly Token[] DateSeparators =
        {
            new Token(TokenKind.Space, ' '),
            new Token(TokenKind.Punctuation, '/'),
            new Token(TokenKind.Punctuation, '-'),
            new Token(TokenKind.Punctuation, '.')
        };

        static readonly Token[] TimeSeparators =
        {
            new Token(TokenKind.Punctuation, ':')
        };

        static readonly Token TimeAM = new Token(TokenKind.Text, "AM");
        static readonly Token TimePM = new Token(TokenKind.Text, "PM");

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="enumerator">The token enumerator to handle the incoming tokens.</param>
        internal CalendarDateParser(TokenEnumerator enumerator) : base(enumerator) { }

        /// <summary>
        /// Attempt to make a date & time.
        /// </summary>
        /// <param name="builder">The date builder that was used to build the date & time.</param>
        /// <returns>true if a date & time could be made, false if not.</returns>
        public bool TryMake(IDateTimeBuilder builder)
        {
            return (TryMake(TryMakeDateTime, builder) && TryMakeEnd())
                || (TryMake(TryMakeDate, builder) && TryMakeEnd())
                || (TryMake(TryMakeTime, builder) && TryMakeEnd())
                || (TryMake(TryMakeSingleDay, builder) && TryMakeEnd());
        }

        /// <summary>
        /// Try to make the date with the given inputs.
        /// </summary>
        /// <param name="builder">The builder to build the date from.</param>
        /// <param name="year">The year to build the date from.</param>
        /// <param name="month">The month to build the date from.</param>
        /// <param name="day">The day to build the date from.</param>
        /// <param name="hour">The hour to build the time from.</param>
        /// <param name="minute">The minute to build the time from.</param>
        /// <param name="second">The second to build the time from.</param>
        /// <returns>true if the date could be made, false if not.</returns>
        static bool TryAccept(IDateTimeBuilder builder, int year, int month, int day, int hour = 0, int minute = 0, int second = 0)
        {
            year = year < 100 ? 2000 + year : year;

            if (day > DateTime.DaysInMonth(year, month))
            {
                return false;
            }

            builder.Year = year;
            builder.Month = month;
            builder.Day = day;
            builder.Hour = hour;
            builder.Minute = minute;
            builder.Second = second;

            return true;
        }

        /// <summary>
        /// Try to make the date based on a relative day to the current day.
        /// </summary>
        /// <param name="builder">The builder to build the date from.</param>
        /// <param name="value">The positive or negative timespan in days to add to the current builder day.</param>
        /// <returns>true if the date could be made, false if not.</returns>
        static bool TryAccept(IDateTimeBuilder builder, TimeSpan value)
        {
            var current = builder.DateTime().Date.Add(value);

            return TryAccept(builder, current.Year, current.Month, current.Day, 0, 0, 0);
        }

        /// <summary>
        /// Try to make the date with the given inputs.
        /// </summary>
        /// <param name="builder">The builder to build the date from.</param>
        /// <param name="hour">The hour to build the time from.</param>
        /// <param name="minute">The minute to build the time from.</param>
        /// <param name="second">The second to build the time from.</param>
        /// <returns>true if the date could be made, false if not.</returns>
        static bool TryAcceptTime(IDateTimeBuilder builder, int hour, int minute, int second)
        {
            return TryAccept(builder, builder.Year, builder.Month, builder.Day, hour, minute, second);
        }

        /// <summary>
        /// Try to make a date.
        /// </summary>
        /// <param name="builder">The builder to make the date from.</param>
        /// <returns>true if the date could be made, false if not.</returns>
        bool TryMakeDateTime(IDateTimeBuilder builder)
        {
            Enumerator.TakeWhile(TokenKind.Space);

            return TryMakeDate(builder) && TryMakeTime(builder);
        }

        /// <summary>
        /// Try to make a date.
        /// </summary>
        /// <param name="builder">The builder to make the date from.</param>
        /// <returns>true if the date could be made, false if not.</returns>
        bool TryMakeDate(IDateTimeBuilder builder)
        {
            Enumerator.TakeWhile(TokenKind.Space);

            return TryMake(TryMakeDayMonthYear, builder)
                || TryMake(TryMakeDayMonth, builder)
                || TryMake(TryMakeMonthYear, builder)
                || TryMake(TryMakeMonth, builder)
                || TryMake(TryMakeYearMonthDay, builder)
                || TryMake(TryMakeYearMonth, builder)
                || TryMake(TryMakeYear, builder)
                || TryMake(TryMakeToday, builder)
                || TryMake(TryMakeTomorrow, builder)
                || TryMake(TryMakeYesterday, builder);
        }

        /// <summary>
        /// Try to make a time.
        /// </summary>
        /// <param name="builder">The builder to make the time from.</param>
        /// <returns>true if the time could be made, false if not.</returns>
        bool TryMakeTime(IDateTimeBuilder builder)
        {
            Enumerator.TakeWhile(TokenKind.Space);

            return TryMake(TryMake12HourMinuteSecond, builder)
                || TryMake(TryMake24HourMinuteSecond, builder)
                || TryMake(TryMake12HourMinute, builder)
                || TryMake(TryMake24HourMinute, builder)
                || TryMake(TryMake12Hour, builder);
        }

        /// <summary>
        /// Try to make a time.
        /// </summary>
        /// <param name="builder">The builder to make the time from.</param>
        /// <returns>true if the time could be made, false if not.</returns>
        bool TryMakeSingleDay(IDateTimeBuilder builder)
        {
            Enumerator.TakeWhile(TokenKind.Space);

            return TryMake(TryMakeDay, builder);
        }

        /// <summary>
        /// Try to make today.
        /// </summary>
        /// <param name="builder">The builder to make the date and time from.</param>
        /// <returns>true if the date and time could be made, false if not.</returns>
        bool TryMakeToday(IDateTimeBuilder builder)
        {
            var token = new Token(TokenKind.Text, "today");

            return Enumerator.Take() == token && TryAccept(builder, TimeSpan.Zero);
        }

        /// <summary>
        /// Try to make tomorrow.
        /// </summary>
        /// <param name="builder">The builder to make the date and time from.</param>
        /// <returns>true if the date and time could be made, false if not.</returns>
        bool TryMakeTomorrow(IDateTimeBuilder builder)
        {
            var token = new Token(TokenKind.Text, "tomorrow");

            return Enumerator.Take() == token && TryAccept(builder, TimeSpan.FromDays(1));
        }

        /// <summary>
        /// Try to make tomorrow.
        /// </summary>
        /// <param name="builder">The builder to make the date and time from.</param>
        /// <returns>true if the date and time could be made, false if not.</returns>
        bool TryMakeYesterday(IDateTimeBuilder builder)
        {
            var token = new Token(TokenKind.Text, "yesterday");

            return Enumerator.Take() == token && TryAccept(builder, TimeSpan.FromDays(-1));
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

            if (TryMakeToken(DateSeparators) == false)
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
            if (TryMakeMonthName(out month) == false)
            {
                return false;
            }

            if (TryMakeToken(DateSeparators) == false)
            {
                return false;
            }

            int year;
            if (TryMakeYearPart(out year) == false)
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
            if (TryMakeMonthNumeric(out month) == false)
            {
                return false;
            }

            if (TryMakeToken(DateSeparators) == false)
            {
                return false;
            }

            int year;
            if (TryMake4DigitYear(out year) == false)
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
            if (TryMakeMonthName(out month) == false)
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
        /// Attempt to make a 12 time in the 12 hour format with minutes and seconds.
        /// </summary>
        /// <param name="builder">The builder to build the time on.</param>
        /// <returns>true if the time could be made, false if not.</returns>
        bool TryMake12HourMinuteSecond(IDateTimeBuilder builder)
        {
            int hour;
            if (TryMake12HourPart(out hour) == false)
            {
                return false;
            }

            Token separator;
            if (TryMakeToken(TimeSeparators, out separator) == false)
            {
                return false;
            }

            int minute;
            if (TryMakeMinutePart(out minute) == false)
            {
                return false;
            }

            if (TryMakeToken(separator) == false)
            {
                return false;
            }

            int second;
            if (TryMakeSecondPart(out second) == false)
            {
                return false;
            }

            if (TryMakeTimePeriod(ref hour) == false)
            {
                return false;
            }

            return TryAcceptTime(builder, hour, minute, second);
        }

        /// <summary>
        /// Attempt to make a time in the 24 hour format with minutes and seconds.
        /// </summary>
        /// <param name="builder">The builder to build the time on.</param>
        /// <returns>true if the time could be made, false if not.</returns>
        bool TryMake24HourMinuteSecond(IDateTimeBuilder builder)
        {
            int hour;
            if (TryMake24HourPart(out hour) == false)
            {
                return false;
            }

            Token separator;
            if (TryMakeToken(TimeSeparators, out separator) == false)
            {
                return false;
            }

            int minute;
            if (TryMakeMinutePart(out minute) == false)
            {
                return false;
            }

            if (TryMakeToken(separator) == false)
            {
                return false;
            }

            int second;
            if (TryMakeSecondPart(out second) == false)
            {
                return false;
            }

            return TryAcceptTime(builder, hour, minute, second);
        }

        /// <summary>
        /// Attempt to make a time in the 12 hour format with minutes.
        /// </summary>
        /// <param name="builder">The builder to build the time on.</param>
        /// <returns>true if the time could be made, false if not.</returns>
        bool TryMake12HourMinute(IDateTimeBuilder builder)
        {
            int hour;
            if (TryMake12HourPart(out hour) == false)
            {
                return false;
            }

            Token separator;
            if (TryMakeToken(TimeSeparators, out separator) == false)
            {
                return false;
            }

            int minute;
            if (TryMakeMinutePart(out minute) == false)
            {
                return false;
            }

            if (TryMakeTimePeriod(ref hour) == false)
            {
                return false;
            }

            return TryAcceptTime(builder, hour, minute, 0);
        }

        /// <summary>
        /// Attempt to make a time in the 24 hour format with minutes.
        /// </summary>
        /// <param name="builder">The builder to build the time on.</param>
        /// <returns>true if the time could be made, false if not.</returns>
        bool TryMake24HourMinute(IDateTimeBuilder builder)
        {
            int hour;
            if (TryMake24HourPart(out hour) == false)
            {
                return false;
            }

            Token separator;
            if (TryMakeToken(TimeSeparators, out separator) == false)
            {
                return false;
            }

            int minute;
            if (TryMakeMinutePart(out minute) == false)
            {
                return false;
            }

            return TryAcceptTime(builder, hour, minute, 0);
        }

        /// <summary>
        /// Attempt to make a time in the 12 hour format with only the hour portion.
        /// </summary>
        /// <param name="builder">The builder to build the time on.</param>
        /// <returns>true if the time could be made, false if not.</returns>
        bool TryMake12Hour(IDateTimeBuilder builder)
        {
            int hour;
            if (TryMake12HourPart(out hour) == false)
            {
                return false;
            }

            if (TryMakeTimePeriod(ref hour) == false)
            {
                return false;
            }

            return TryAcceptTime(builder, hour, 0, 0);
        }

        /// <summary>
        /// Attempt to make the 12 hour part of the time.
        /// </summary>
        /// <param name="hour">The hour part of a time.</param>
        /// <returns>true if a hour part could be made, false if not.</returns>
        bool TryMake12HourPart(out int hour)
        {
            if (TryMake(TryMakeNumber, 2, out hour) == false && TryMake(TryMakeNumber, 1, out hour) == false)
            {
                return false;
            }

            return hour >= 0 && hour <= 12;
        }

        /// <summary>
        /// Attempt to make the 24 hour part of the time.
        /// </summary>
        /// <param name="hour">The hour part of a time.</param>
        /// <returns>true if a hour part could be made, false if not.</returns>
        bool TryMake24HourPart(out int hour)
        {
            if (TryMake(TryMakeNumber, 2, out hour) == false && TryMake(TryMakeNumber, 1, out hour) == false)
            {
                return false;
            }

            return hour >= 0 && hour <= 23;
        }

        /// <summary>
        /// Attempt to make the minute part of the time.
        /// </summary>
        /// <param name="minute">The minute part of a time.</param>
        /// <returns>true if a minute part could be made, false if not.</returns>
        bool TryMakeMinutePart(out int minute)
        {
            return TryMakeNumber(2, out minute) && minute >= 0 && minute <= 59;
        }

        /// <summary>
        /// Attempt to make the second part of the time.
        /// </summary>
        /// <param name="second">The second part of a time.</param>
        /// <returns>true if a second part could be made, false if not.</returns>
        bool TryMakeSecondPart(out int second)
        {
            return TryMakeNumber(2, out second) && second >= 0 && second <= 59;
        }

        /// <summary>
        /// Attempt to make the time period whilst adjusting the hour.
        /// </summary>
        /// <param name="hour">The hour to adjust according to the time period.</param>
        /// <returns>true if a time period could be made, false if not.</returns>
        bool TryMakeTimePeriod(ref int hour)
        {
            Enumerator.TakeWhile(TokenKind.Space);

            Token token;
            if (TryMakeToken(new[] { TimeAM, TimePM }, out token) == false)
            {
                return false;
            }

            if (token == TimePM && hour < 12)
            {
                hour += 12;
            }

            if (token == TimeAM && hour == 12)
            {
                hour = 0;
            }

            return true;
        }
    }
}
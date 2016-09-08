using NaturalDate.Text;

namespace NaturalDate
{
    internal sealed class TimeParser : TokenParser, IDateTimeParser
    {
        static readonly Token TimeSeparator = new Token(TokenKind.Punctuation, ':');
        static readonly Token TimeAM = new Token(TokenKind.Text, "AM");
        static readonly Token TimePM = new Token(TokenKind.Text, "PM");

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="enumerator">The token enumerator to handle the incoming tokens.</param>
        internal TimeParser(TokenEnumerator enumerator) : base(enumerator) { }

        /// <summary>
        /// Attempt to make a date & time.
        /// </summary>
        /// <param name="builder">The date builder that was used to build the date & time.</param>
        /// <returns>true if a date & time could be made, false if not.</returns>
        public bool TryMake(IDateTimeBuilder builder)
        {
            int hour;
            if (TryMakeHourPart(out hour) == false)
            {
                return false;
            }

            if (TryMakeAmOrPm(ref hour) || TryMakeEnd())
            {
                return TryMake(builder, hour, 0, 0);
            }

            if (TryMakeToken(TimeSeparator) == false)
            {
                return false;
            }

            int minute;
            if (TryMakeMinutePart(out minute) == false)
            {
                return false;
            }

            if (TryMakeAmOrPm(ref hour) || TryMakeEnd())
            {
                return TryMake(builder, hour, minute, 0);
            }

            if (TryMakeToken(TimeSeparator) == false)
            {
                return false;
            }

            int second;
            if (TryMakeSecondPart(out second) == false)
            {
                return false;
            }

            if (CanMakeAmOrPm() && TryMakeAmOrPm(ref hour) == false)
            {
                return false;
            }

            return TryMake(builder, hour, minute, second);
        }

        /// <summary>
        /// Attempt to make a date & time.
        /// </summary>
        /// <param name="builder">The date builder that was used to build the date & time.</param>
        /// <param name="hour">The hour part of the time.</param>
        /// <param name="minute">The minute part of the time.</param>
        /// <param name="second">The second part of the time.</param>
        /// <returns>true if a date & time could be made, false if not.</returns>
        static bool TryMake(IDateTimeBuilder builder, int hour, int minute, int second)
        {
            builder.Hour = hour;
            builder.Minute = minute;
            builder.Second = second;

            return true;
        }

        /// <summary>
        /// Attempt to make the hour part of the date.
        /// </summary>
        /// <param name="hour">The hour part of a date.</param>
        /// <returns>true if a hour part could be made, false if not.</returns>
        bool TryMakeHourPart(out int hour)
        {
            return TryMakeNumeric(1, 2, out hour) && hour >= 0 && hour < 24;
        }

        /// <summary>
        /// Attempt to make the minute part of the date.
        /// </summary>
        /// <param name="minute">The minute part of a date.</param>
        /// <returns>true if a minute part could be made, false if not.</returns>
        bool TryMakeMinutePart(out int minute)
        {
            return TryMakeNumeric(1, 2, out minute) && minute >= 0 && minute <= 59;
        }

        /// <summary>
        /// Attempt to make the second part of the date.
        /// </summary>
        /// <param name="second">The second part of a date.</param>
        /// <returns>true if a second part could be made, false if not.</returns>
        bool TryMakeSecondPart(out int second)
        {
            return TryMakeNumeric(1, 2, out second) && second >= 0 && second <= 59;
        }

        /// <summary>
        /// Returns a value indicating whether or not an AM or PM token can be made.
        /// </summary>
        /// <returns>true if an AM or PM token can be made, false if not.</returns>
        bool CanMakeAmOrPm()
        {
            var count = Enumerator.SkipWhile(TokenKind.Space);

            return CanMakeToken(TimeAM, count) || CanMakeToken(TimePM, count);
        }

        /// <summary>
        /// Attempt to make the AM or PM specifier.
        /// </summary>
        /// <param name="hour">The hour that was potentially adjusted.</param>
        /// <returns>true if a AM or PM part could be made, false if not.</returns>
        bool TryMakeAmOrPm(ref int hour)
        {
            var count = Enumerator.SkipWhile(TokenKind.Space);

            if (TryMakeToken(TimeAM, count))
            {
                return hour < 12;
            }

            if (TryMakeToken(TimePM, count) && hour < 12)
            {
                hour = hour + 12;
                return true;
            }

            return false;
        }
    }
}
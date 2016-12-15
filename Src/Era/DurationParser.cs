using System;
using System.Collections.Generic;
using Era.Text;

namespace Era
{
    internal sealed class DurationParser : TokenParser
    {
        static readonly Token DecimalPointToken = new Token(TokenKind.Punctuation, '.');

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="enumerator">The token enumerator to handle the incoming tokens.</param>
        internal DurationParser(TokenEnumerator enumerator) : base(enumerator) { }

        /// <summary>
        /// Attempt to make a timespan from the token enumerator.
        /// </summary>
        /// <param name="builder">The builder to apply the values to.</param>
        /// <returns>true if a timespan could be made, false if not.</returns>
        internal bool TryMake(IDurationBuilder builder)
        {
            return TryMakeYear(builder);
        }

        /// <summary>
        /// Attempt to make a decimal month.
        /// </summary>
        /// <param name="builder">The time span builder to updated if the value was made.</param>
        /// <returns>true if the decimal duration was made, false if not.</returns>
        bool TryMakeYear(IDurationBuilder builder)
        {
            decimal duration;
            if (TryMake(TryMakeSegment, TimePart.Year, out duration))
            {
                builder.Year = duration;
            }

            return TryMakeMonth(builder);
        }

        /// <summary>
        /// Attempt to make a decimal month.
        /// </summary>
        /// <param name="builder">The time span builder to updated if the value was made.</param>
        /// <returns>true if the decimal duration was made, false if not.</returns>
        bool TryMakeMonth(IDurationBuilder builder)
        {
            decimal duration;
            if (TryMake(TryMakeSegment, TimePart.Month, out duration))
            {
                builder.Month = duration;
            }

            return TryMakeDay(builder);
        }

        /// <summary>
        /// Attempt to make a decimal day.
        /// </summary>
        /// <param name="builder">The time span builder to updated if the value was made.</param>
        /// <returns>true if the decimal duration was made, false if not.</returns>
        bool TryMakeDay(IDurationBuilder builder)
        {
            decimal duration;
            if (TryMake(TryMakeSegment, TimePart.Day, out duration))
            {
                builder.Day = duration;
            }

            return TryMakeHour(builder);
        }

        /// <summary>
        /// Attempt to make a decimal hour.
        /// </summary>
        /// <param name="builder">The time span builder to updated if the value was made.</param>
        /// <returns>true if the decimal duration was made, false if not.</returns>
        bool TryMakeHour(IDurationBuilder builder)
        {
            decimal duration;
            if (TryMake(TryMakeSegment, TimePart.Hour, out duration))
            {
                builder.Hour = duration;
            }

            return TryMakeMinute(builder);
        }

        /// <summary>
        /// Attempt to make a decimal minute.
        /// </summary>
        /// <param name="builder">The time span builder to updated if the value was made.</param>
        /// <returns>true if the decimal duration was made, false if not.</returns>
        bool TryMakeMinute(IDurationBuilder builder)
        {
            decimal duration;
            if (TryMake(TryMakeSegment, TimePart.Minute, out duration))
            {
                builder.Minute = duration;
            }
            
            return TryMakeSecond(builder);
        }
        
        /// <summary>
        /// Attempt to make a decimal second.
        /// </summary>
        /// <param name="builder">The time span builder to updated if the value was made.</param>
        /// <returns>true if the decimal duration was made, false if not.</returns>
        bool TryMakeSecond(IDurationBuilder builder)
        {
            decimal duration;
            if (TryMake(TryMakeSegment, TimePart.Second, out duration))
            {
                builder.Second = duration;
            }

            return TryMakeEnd();
        }

        /// <summary>
        /// Attempt to make a decimal duration segment.
        /// </summary>
        /// <param name="expected">The time part that was expected.</param>
        /// <param name="duration">The decimal duration that was made.</param>
        /// <returns>true if the decimal duration was made, false if not.</returns>
        bool TryMakeSegment(TimePart expected, out decimal duration)
        {
            Enumerator.TakeWhile(TokenKind.Space);

            return TryMakeDecimal(out duration) && TryMakeTimePart(expected);
        }

        /// <summary>
        /// Attempt to match a time part.
        /// </summary>
        /// <param name="expected">The time part that is expected.</param>
        /// <returns>true if the time part could be matched, false if not.</returns>
        bool TryMakeTimePart(TimePart expected)
        {
            var dictionary = new Dictionary<string, TimePart>
            {
                { "y", TimePart.Year },
                { "mo", TimePart.Month },
                { "d", TimePart.Day },
                { "h", TimePart.Hour },
                { "m", TimePart.Minute },
                { "s", TimePart.Second },
            };

            Enumerator.TakeWhile(TokenKind.Space);

            TimePart part;
            return dictionary.TryGetValue(Enumerator.Take().Text, out part) && part == expected;
        }

        /// <summary>
        /// Attempt to make a decimal.
        /// </summary>
        /// <param name="d">The decimal that was made.</param>
        /// <returns>true if a number could be made, false if not.</returns>
        bool TryMakeDecimal(out decimal d)
        {
            int precision;
            double scale;
            if (TryMakeNumber(out precision, out scale))
            {
                d = (decimal)(precision + scale);
                return true;
            }

            d = 0;
            return false;
        }

        /// <summary>
        /// Attempt to make a number.
        /// </summary>
        /// <param name="precision">The whole number portion.</param>
        /// <param name="scale">The scale portion of the number.</param>
        /// <returns>true if a number could be made, false if not.</returns>
        bool TryMakeNumber(out int precision, out double scale)
        {
            scale = 0;

            if (TryMakePrecision(out precision) == false)
            {
                return false;
            }

            if (Enumerator.Peek() != DecimalPointToken)
            {
                return true;
            }

            Enumerator.Take();

            return TryMakeScale(out scale);
        }

        /// <summary>
        /// Attempt to make the precision portion of the number.
        /// </summary>
        /// <param name="precision">The precision portion that was made.</param>
        /// <returns>true if the precision part of the number could be made, false if not.</returns>
        bool TryMakePrecision(out int precision)
        {
            precision = 0;

            if (Enumerator.Peek().Kind != TokenKind.Digit)
            {
                return false;
            }

            while (Enumerator.Peek().Kind == TokenKind.Digit)
            {
                precision = (precision * 10) + Enumerator.Take().Text[0] - 48;
            }

            return true;
        }

        /// <summary>
        /// Attempt to make the scale portion of the number.
        /// </summary>
        /// <param name="scale">The scale portion that was made.</param>
        /// <returns>true if the scale part of the number could be made, false if not.</returns>
        bool TryMakeScale(out double scale)
        {
            scale = 0;

            if (Enumerator.Peek().Kind != TokenKind.Digit)
            {
                return false;
            }

            var count = 1;
            while (Enumerator.Peek().Kind == TokenKind.Digit)
            {
                scale = scale + (Enumerator.Take().Text[0] - 48) / Math.Pow(10, count);
                count++;
            }

            return true;
        }
    }
}
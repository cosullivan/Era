using System;
using NaturalDate.Text;

namespace NaturalDate
{
    internal sealed class NaturalDateParser : TokenParser, IDateParser
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="enumerator">The token enumerator to handle the incoming tokens.</param>
        internal NaturalDateParser(TokenEnumerator enumerator) : base(enumerator) { }

        /// <summary>
        /// Attempt to make a date.
        /// </summary>
        /// <param name="builder">The date builder that was is to build the date.</param>
        /// <returns>true if a date could be made, false if not.</returns>
        public bool TryMakeDate(IDateTimeBuilder builder)
        {
            return TryMakeNow(builder) 
                || TryMakeToday(builder) 
                || TryMakeTomorrow(builder)
                || TryMakeDayOfWeek(builder);
        }

        /// <summary>
        /// Attempt to make a natural day of the week.
        /// </summary>
        /// <param name="builder">The date builder that was is to build the date.</param>
        /// <returns>true if a date could be made, false if not.</returns>
        bool TryMakeNow(IDateTimeBuilder builder)
        {
            if (Enumerator.Peek() == new Token(TokenKind.Text, "now"))
            {
                Enumerator.Take();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Attempt to make a natural day of the week.
        /// </summary>
        /// <param name="builder">The date builder that was is to build the date.</param>
        /// <returns>true if a date could be made, false if not.</returns>
        bool TryMakeToday(IDateTimeBuilder builder)
        {
            if (Enumerator.Peek() != new Token(TokenKind.Text, "today"))
            {
                return false;
            }

            Enumerator.Take();

            builder.Hour = 0;
            builder.Minute = 0;
            builder.Second = 0;

            return true;
        }

        /// <summary>
        /// Attempt to make a natural day of the week.
        /// </summary>
        /// <param name="builder">The date builder that was is to build the date.</param>
        /// <returns>true if a date could be made, false if not.</returns>
        bool TryMakeTomorrow(IDateTimeBuilder builder)
        {
            if (Enumerator.Peek() != new Token(TokenKind.Text, "tomorrow"))
            {
                return false;
            }

            Enumerator.Take();

            builder.Hour = 0;
            builder.Minute = 0;
            builder.Second = 0;

            IncrementByDays(builder, 1);

            return true;
        }

        /// <summary>
        /// Increment the builder by a given number of days.
        /// </summary>
        /// <param name="builder">The builder to increment.</param>
        /// <param name="days">The number of days to increment the builder by.</param>
        static void IncrementByDays(IDateTimeBuilder builder, int days)
        {
            if (builder.Day + days < DateTime.DaysInMonth(builder.Year, builder.Month))
            {
                builder.Day += days;
                return;
            }

            throw new NotImplementedException();
        }

        /// <summary>
        /// Attempt to make a natural day of the week.
        /// </summary>
        /// <param name="builder">The date builder that was is to build the date.</param>
        /// <returns>true if a date could be made, false if not.</returns>
        bool TryMakeDayOfWeek(IDateTimeBuilder builder)
        {
            DayOfWeek dayOfWeek;
            if (Enumerator.Peek().Kind != TokenKind.Text || Enum.TryParse(Enumerator.Peek().Text, true, out dayOfWeek) == false)
            {
                return false;
            }

            Enumerator.Take();

            var current = new DateTime(builder.Year, builder.Month, builder.Day, 0, 0, 0);

            if (current.DayOfWeek >= dayOfWeek)
            {
                current = current.AddDays(7 - (current.DayOfWeek - dayOfWeek));
            }
            else
            {
                current = current.AddDays(dayOfWeek - current.DayOfWeek);
            }

            builder.Year = current.Year;
            builder.Month = current.Month;
            builder.Day = current.Day;
            builder.Hour = 0;
            builder.Minute = 0;
            builder.Second = 0;

            return true;
        }
    }
}
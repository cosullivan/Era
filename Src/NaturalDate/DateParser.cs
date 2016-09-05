using System;
using NaturalDate.Text;

namespace NaturalDate
{
    public sealed class DateParser : Parser
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="reader">The token reader to read the input from.</param>
        public DateParser(TokenReader reader) : base(new TokenEnumerator(reader)) { }

        /// <summary>
        /// Attempt to make a date.
        /// </summary>
        /// <param name="date">The date that was made from the input tokens.</param>
        /// <returns>true if a date could be made, false if not.</returns>
        public bool TryMakeDate(out DateTime date)
        {
            if (TryMake(TryMakeCalendarDate, out date) || TryMake(TryMakeFriendlyDate, out date))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Attempt to make a date from a standard calendar date syntax.
        /// </summary>
        /// <param name="date">The date that was made from the input tokens.</param>
        /// <returns>true if a date could be made, false if not.</returns>
        internal bool TryMakeCalendarDate(out DateTime date)
        {
            //return new CalendarDateParser(Enumerator).TryMakeDate(out date);
            date = DateTime.MaxValue;
            return false;
        }

        /// <summary>
        /// Attempt to make a date from the friendly text syntax.
        /// </summary>
        /// <param name="date">The date that was made from the input tokens.</param>
        /// <returns>true if a date could be made, false if not.</returns>
        internal bool TryMakeFriendlyDate(out DateTime date)
        {
            return new FriendlyDateParser(Enumerator).TryMakeDate(out date);
        }

        /// <summary>
        /// Attempt to make a time from the friendly text syntax.
        /// </summary>
        /// <param name="time">The time that was made from the input tokens.</param>
        /// <returns>true if a time could be made, false if not.</returns>
        internal bool TryMakeTime(out DateTime time)
        {
            return new TimeParser(Enumerator).TryMakeTime(out time);
        }
    }
}
using System;
using NaturalDate.Text;

namespace NaturalDate
{
    internal sealed class TimeParser : TokenParser
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="enumerator">The token enumerator to handle the incoming tokens.</param>
        internal TimeParser(TokenEnumerator enumerator) : base(enumerator) { }

        /// <summary>
        /// Attempt to make a time.
        /// </summary>
        /// <param name="time">The time that was made from the input tokens.</param>
        /// <returns>true if a time could be made, false if not.</returns>
        public bool TryMakeTime(out DateTime time)
        {
            time = DateTime.Now;

            int hour;
            if (TryMake(TryMakeHourPart, out hour) == false)
            {
                return false;
            }
            
            return false;
        }

        /// <summary>
        /// Attempt to make the hour part of the time.
        /// </summary>
        /// <param name="hour">The hour part of a time.</param>
        /// <returns>true if the hour part could be made, false if not.</returns>
        internal bool TryMakeHourPart(out int hour)
        {
            hour = 0;

            var token = Enumerator.Take();
            if (token.Kind != TokenKind.Number || token.LeadingZeros() > 1)
            {
                return false;
            }

            hour = token.AsInteger();

            return hour >= 0 && hour <= 23;
        }
    }
}
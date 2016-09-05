using System;
using System.Collections.Generic;
using NaturalDate.Text;

namespace NaturalDate
{
    internal sealed class FriendlyDateParser : Parser
    {
        static readonly Dictionary<string, Func<DateTime>> Callbacks = new Dictionary<string, Func<DateTime>>(StringComparer.OrdinalIgnoreCase)
        {
            { "now", Now },
            { "today", Today }
        };

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="enumerator">The token enumerator to handle the incoming tokens.</param>
        internal FriendlyDateParser(TokenEnumerator enumerator) : base(enumerator) { }

        /// <summary>
        /// Attempt to make a date.
        /// </summary>
        /// <param name="date">The date that was made from the input tokens.</param>
        /// <returns>true if a date could be made, false if not.</returns>
        public bool TryMakeDate(out DateTime date)
        {
            date = DateTime.Now;

            if (Enumerator.Peek().Kind != TokenKind.Text)
            {
                return false;
            }

            Func<DateTime> callback;
            if (Callbacks.TryGetValue(Enumerator.Take().Text, out callback) == false)
            {
                return false;
            }

            date = callback();
            return true;
        }

        /// <summary>
        /// Returns the current date and time.
        /// </summary>
        /// <returns>The current date and time.</returns>
        static DateTime Now()
        {
            return DateTime.Now;
        }

        /// <summary>
        /// Returns the current date.
        /// </summary>
        /// <returns>The current date.</returns>
        static DateTime Today()
        {
            return DateTime.Today;
        }
    }
}
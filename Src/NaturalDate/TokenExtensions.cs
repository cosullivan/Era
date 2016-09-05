using System;
using NaturalDate.Text;

namespace NaturalDate
{
    internal static class TokenExtensions
    {
        /// <summary>
        /// Returns the number of leading zeros on the number token.
        /// </summary>
        /// <param name="token">The token to return the number of leading zeros.</param>
        /// <returns>The number of leading zeros on the numeric token.</returns>
        internal static int LeadingZeros(this Token token)
        {
            if (token.Kind != TokenKind.Number)
            {
                throw new ArgumentException(nameof(token));
            }

            var count = -1;
            while (token.Text[++count] == '0') { }

            return count;
        }

        /// <summary>
        /// Returns the token as an integer.
        /// </summary>
        /// <param name="token">The token to return as an integer.</param>
        /// <returns>The integer value that represents the token.</returns>
        internal static int AsInteger(this Token token)
        {
            if (token.Kind != TokenKind.Number)
            {
                throw new ArgumentException(nameof(token));
            }

            return Int32.Parse(token.Text);
        }
    }
}

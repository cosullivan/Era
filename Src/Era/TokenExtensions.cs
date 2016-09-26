﻿using System;
using Era.Text;

namespace Era
{
    internal static class TokenExtensions
    {
        /// <summary>
        /// Returns the token as an integer.
        /// </summary>
        /// <param name="token">The token to return as an integer.</param>
        /// <returns>The integer value that represents the token.</returns>
        internal static int AsInteger(this Token token)
        {
            if (token.Kind != TokenKind.Digit)
            {
                throw new ArgumentException(nameof(token));
            }

            return Int32.Parse(token.Text);
        }
    }
}
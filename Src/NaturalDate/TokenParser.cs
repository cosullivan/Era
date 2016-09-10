using NaturalDate.Text;

namespace NaturalDate
{
    public abstract class TokenParser
    {
        protected delegate bool TryMakeDelegate1<in TIn>(TIn p1);
        protected delegate bool TryMakeDelegate2<TOut>(out TOut p1);
        protected delegate bool TryMakeDelegate3<in TIn, TOut>(TIn p1, out TOut p2);

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="enumerator">The token enumerator to handle the incoming tokens.</param>
        protected TokenParser(TokenEnumerator enumerator)
        {
            Enumerator = enumerator;
        }

        /// <summary>
        /// Attempt to make the end of stream token.
        /// </summary>
        /// <returns>true if the enumerator has reached the end, false if not.</returns>
        protected bool TryMakeEnd()
        {
            var count = Enumerator.SkipWhile(TokenKind.Space);

            if (Enumerator.Peek(count) == Token.None)
            {
                Enumerator.Take(count);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Attempt to make one of the specified tokens.
        /// </summary>
        /// <param name="tokens">The list of tokens to test for a match.</param>
        /// <param name="found">The token that was found.</param>
        /// <returns>true if one of the the specific tokens was made, false if not.</returns>
        protected bool TryMakeToken(Token[] tokens, out Token found)
        {
            found = Token.None;
            
            foreach (var token in tokens)
            {
                if (TryMakeToken(token))
                {
                    found = token;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Attempt to make one of the specified tokens.
        /// </summary>
        /// <param name="tokens">The list of tokens to test for a match.</param>
        /// <returns>true if one of the the specific tokens was made, false if not.</returns>
        protected bool TryMakeToken(params Token[] tokens)
        {
            foreach (var token in tokens)
            {
                if (TryMakeToken(token))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Attempt to make the specific token.
        /// </summary>
        /// <param name="token">The specific token to match at the current point.</param>
        /// <param name="offset">The offset into the future at which to make the match.</param>
        /// <returns>true if the specific separator token was made, false if not.</returns>
        protected bool CanMakeToken(Token token, int offset = 0)
        {
            return Enumerator.Peek(offset) == token;
        }

        /// <summary>
        /// Attempt to make the specific token.
        /// </summary>
        /// <param name="token">The specific token to match at the current point.</param>
        /// <param name="offset">The offset into the future at which to make the match.</param>
        /// <returns>true if the specific separator token was made, false if not.</returns>
        protected bool TryMakeToken(Token token, int offset = 0)
        {
            if (Enumerator.Peek(offset) == token)
            {
                Enumerator.Take(offset + 1);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Attempt to make a numeric value whilst considering leading zeros.
        /// </summary>
        /// <param name="length">The minimum length (including leading zeros).</param>
        /// <param name="value">The value that was made.</param>
        /// <returns>true if a value with the given length could be made, false if not.</returns>
        protected bool TryMakeNumeric(int length, out int value)
        {
            return TryMakeNumeric(length, length, out value);
        }

        /// <summary>
        /// Attempt to make a numeric value whilst considering leading zeros.
        /// </summary>
        /// <param name="minLength">The minimum length (including leading zeros).</param>
        /// <param name="maxLength">The maximum length (including leading zeros).</param>
        /// <param name="value">The value that was made.</param>
        /// <returns>true if a value with the given length could be made, false if not.</returns>
        protected bool TryMakeNumeric(int minLength, int maxLength, out int value)
        {
            value = 0;

            var token = Enumerator.Peek();
            if (token.Kind == TokenKind.Number && token.Text.Length >= minLength && token.Text.Length <= maxLength)
            {
                Enumerator.Take();
                value = token.AsInteger();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Try to make a callback in a transactional way.
        /// </summary>
        /// <param name="delegate">The callback to perform the match.</param>
        /// <param name="p1">The parameter to pass to the matching function.</param>
        /// <returns>true if the match could be made, false if not.</returns>
        protected bool TryMake<TIn>(TryMakeDelegate1<TIn> @delegate, TIn p1)
        {
            var index = Enumerator.Index;

            if (@delegate(p1) == false)
            {
                Enumerator.Index = index;
                return false;
            }

            return true;
        }

        /// <summary>
        /// Try to make a callback in a transactional way.
        /// </summary>
        /// <param name="delegate">The callback to perform the match.</param>
        /// <param name="p1">The parameter that was returned from the matching function.</param>
        /// <returns>true if the match could be made, false if not.</returns>
        protected bool TryMake<TOut>(TryMakeDelegate2<TOut> @delegate, out TOut p1)
        {
            var index = Enumerator.Index;

            if (@delegate(out p1) == false)
            {
                Enumerator.Index = index;
                return false;
            }

            return true;
        }

        /// <summary>
        /// Try to make a callback in a transactional way.
        /// </summary>
        /// <param name="delegate">The callback to perform the match.</param>
        /// <param name="p1">The parameter to pass to the matching function.</param>
        /// <param name="p2">The parameter that was returned from the matching function.</param>
        /// <returns>true if the match could be made, false if not.</returns>
        protected bool TryMake<TIn, TOut>(TryMakeDelegate3<TIn, TOut> @delegate, TIn p1, out TOut p2)
        {
            var index = Enumerator.Index;

            if (@delegate(p1, out p2) == false)
            {
                Enumerator.Index = index;
                return false;
            }

            return true;
        }

        /// <summary>
        /// Returns the enumerator to handle the incoming tokens.
        /// </summary>
        protected TokenEnumerator Enumerator { get; }
    }
}
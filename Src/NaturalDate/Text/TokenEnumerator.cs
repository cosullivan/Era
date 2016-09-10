using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace NaturalDate.Text
{
    public sealed class TokenEnumerator
    {
        readonly Token[] _tokens;
        
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="tokenizer">The tokenizer to retrieve the tokens from.</param>
        public TokenEnumerator(IEnumerable<Token> tokenizer)
        {
            _tokens = tokenizer.ToArray();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="tokenReader">The token reader to pull the tokens from.</param>
        public TokenEnumerator(TokenReader tokenReader)
        {
            var tokens = new List<Token>();

            var token = tokenReader.NextToken();
            while (token != Token.None)
            {
                tokens.Add(token);

                token = tokenReader.NextToken();
            }

            _tokens = tokens.ToArray();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="tokens">The tokens to enumerate over.</param>
        internal TokenEnumerator(params Token[] tokens)
        {
            _tokens = tokens;
        }

        /// <summary>
        /// Peek at a token in the stream.
        /// </summary>
        /// <param name="count">The number of tokens to look ahead.</param>
        /// <returns>The token at the given number of tokens past the current index, or Token.None if no token exists.</returns>
        public Token Peek(int count = 0)
        {
            if (Index + count < _tokens.Length)
            {
                return _tokens[Index + count];
            }

            return Token.None;
        }

        /// <summary>
        /// Skips the tokens whilst they match the predicate.
        /// </summary>
        /// <param name="predicate">The predicate to use for evaluating whether or not to skip a token.</param>
        /// <returns>The offset that was skipped until.</returns>
        public int SkipWhile(Func<Token, bool> predicate)
        {
            var count = 0;

            while (predicate(Peek(count)))
            {
                count++;
            }

            return count;
        }

        /// <summary>
        /// Skips the tokens whilst the match the given token kind.
        /// </summary>
        /// <param name="kind">The token kind to test against to determine whether the token should be skipped.</param>
        /// <returns>The offset that was skipped until.</returns>
        public int SkipWhile(TokenKind kind)
        {
            return SkipWhile(t => t.Kind == kind);
        }

        /// <summary>
        /// Take tokens in the stream while the given predicate is true.
        /// </summary>
        /// <param name="predicate">The predicate to use for evaluating whether or not to consume a token.</param>
        public void TakeWhile(Func<Token, bool> predicate)
        {
            while (predicate(Peek()))
            {
                Take();
            }
        }

        /// <summary>
        /// Take tokens in the stream while the token kind is the same as the supplied kind.
        /// </summary>
        /// <param name="kind">The token kind to test against to determine whether the token should be consumed.</param>
        public void TakeWhile(TokenKind kind)
        {
            TakeWhile(t => t.Kind == kind);
        }

        /// <summary>
        /// Take the given number of tokens.
        /// </summary>
        /// <param name="count">The number of tokens to consume.</param>
        /// <returns>The last token that was consumed.</returns>
        public Token Take(int count = 1)
        {
            Index += count;

            return Peek(-1);
        }
        
        /// <summary>
        /// Gets the current index into the enumerator.
        /// </summary>
        public int Index { get; internal set; }
    }
}
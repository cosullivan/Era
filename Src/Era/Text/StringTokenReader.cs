using System;

namespace Era.Text
{
    public sealed class StringTokenReader : TokenReader
    {
        readonly string _input;
        int _index = -1;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="input">The input string.</param>
        public StringTokenReader(string input)
        {
            _input = input;
        }

        /// <summary>
        /// Reads the next token.
        /// </summary>
        /// <returns>The next token that was read.</returns>
        public override Token NextToken()
        {
            if (_index > -1 && IsEof(_index))
            {
                return Token.None;
            }

            _index++;

            if (Char.IsLetter(_input[_index]))
            {
                return TextToken();
            }

            if (Char.IsDigit(_input[_index]))
            {
                return new Token(TokenKind.Digit, _input[_index]);
            }

            if (Char.IsPunctuation(_input[_index]))
            {
                return new Token(TokenKind.Punctuation, _input[_index]);
            }

            if (Char.IsSymbol(_input[_index]))
            {
                return new Token(TokenKind.Symbol, _input[_index]);
            }

            if (Char.IsWhiteSpace(_input[_index]))
            {
                return new Token(TokenKind.Space, _input[_index]);
            }

            return new Token(TokenKind.Other, _input[_index]);
        }

        /// <summary>
        /// Returns a Text token from the current position.
        /// </summary>
        /// <returns>The text token that was found at the current position.</returns>
        Token TextToken()
        {
            var start = _index;

            while (IsEof(_index) == false && Char.IsLetterOrDigit(_input[_index + 1]))
            {
                _index++;
            }

            return new Token(TokenKind.Text, _input.Substring(start, (_index - start) + 1));
        }

        /// <summary>
        /// Returns a value indicating whether or not the given index is currently at the end of the input string.
        /// </summary>
        /// <param name="index">The index to test.</param>
        /// <returns>true if the given index is at the end of the input, false if not.</returns>
        bool IsEof(int index)
        {
            return index + 1 >= _input.Length;
        }
    }
}
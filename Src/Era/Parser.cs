using System;
using Era.Text;

namespace Era
{
    public sealed class Parser : TokenParser
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="enumerator">The token enumerator to handle the incoming tokens.</param>
        internal Parser(TokenEnumerator enumerator) : base(enumerator) { }

        /// <summary>
        /// Attempt to parse the given input into a usable format.
        /// </summary>
        /// <typeparam name="T">The element type to parse the input to.</typeparam>
        /// <param name="input">The input string to attempt to parse.</param>
        /// <param name="builder">The date builder to build the date into.</param>
        /// <returns>true if the input could be parsed, false if not.</returns>
        public static bool TryParse<T>(string input, IDateTimeBuilder<T> builder)
        {
            var parser = new DateTimeParser(new TokenEnumerator(new StringTokenReader(input)));

            return parser.TryMake(builder);
        }

        /// <summary>
        /// Attempt to parse the given input into a usable format.
        /// </summary>
        /// <param name="input">The input string to attempt to parse.</param>
        /// <param name="value">The value that was built.</param>
        /// <returns>true if the input could be parsed, false if not.</returns>
        public static bool TryParse(string input, out DateTime value)
        {
            var builder = new DateTimeBuilder(DateTime.Now);

            if (TryParse(input, builder))
            {
                value = builder.Build();
                return true;
            }

            value = default(DateTime);
            return false;
        }

        /// <summary>
        /// Attempt to parse the given input into a usable format.
        /// </summary>
        /// <typeparam name="T">The element type to parse the input to.</typeparam>
        /// <param name="input">The input string to attempt to parse.</param>
        /// <param name="builder">The duration builder to build the duration into.</param>
        /// <returns>true if the input could be parsed, false if not.</returns>
        public static bool TryParse<T>(string input, IDurationBuilder<T> builder)
        {
            var parser = new DurationParser(new TokenEnumerator(new StringTokenReader(input)));

            return parser.TryMake(builder);
        }

        /// <summary>
        /// Attempt to parse the given input into a usable format.
        /// </summary>
        /// <param name="input">The input string to attempt to parse.</param>
        /// <param name="value">The value that was built.</param>
        /// <returns>true if the input could be parsed, false if not.</returns>
        public static bool TryParse(string input, out TimeSpan value)
        {
            var builder = new DurationBuilder(DateTime.Now);

            if (TryParse(input, builder))
            {
                value = builder.Build();
                return true;
            }

            value = default(TimeSpan);
            return false;
        }
    }
}
using System;
using NaturalDate.Text;

namespace NaturalDate
{
    public sealed class Parser
    {
        readonly TokenEnumerator _enumerator;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="enumerator">The token enumerator to handle the incoming tokens.</param>
        internal Parser(TokenEnumerator enumerator)
        {
            _enumerator = enumerator;
        }

        /// <summary>
        /// Attempt to parse the given input into a usable format.
        /// </summary>
        /// <typeparam name="T">The element type to parse the input to.</typeparam>
        /// <param name="input">The input string to attempt to parse.</param>
        /// <param name="builder">The date builder to build the date into.</param>
        /// <returns>true if the input could be parsed, false if not.</returns>
        public static bool TryParse<T>(string input, IDateTimeBuilder<T> builder)
        {
            var parser = new Parser(new TokenEnumerator(new StringTokenReader(input)));

            return parser.TryMakeDate(builder);
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
        /// Attempt to make a date.
        /// </summary>
        /// <param name="builder">The date builder that was used to build the date.</param>
        /// <returns>true if a date could be made, false if not.</returns>
        bool TryMakeDate(IDateTimeBuilder builder)
        {
            return false;
        }
    }
}

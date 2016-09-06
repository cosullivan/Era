using System;
using System.Collections.Generic;
using NaturalDate.Text;

namespace NaturalDate
{
    internal abstract class CalendarDateParser : TokenParser, IDateParser
    {
        static readonly Token[] DateSeparators =
        {
            new Token(TokenKind.Space, ' '),
            new Token(TokenKind.Punctuation, '/'),
            new Token(TokenKind.Punctuation, '-'),
            new Token(TokenKind.Punctuation, '.')
        };

        Token _separator = Token.None;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="enumerator">The token enumerator to handle the incoming tokens.</param>
        protected CalendarDateParser(TokenEnumerator enumerator) : base(enumerator) { }

        /// <summary>
        /// Attempt to make a date.
        /// </summary>
        /// <param name="builder">The date builder that was used to build the date.</param>
        /// <returns>true if a date could be made, false if not.</returns>
        public abstract bool TryMakeDate(IDateBuilder builder);

        /// <summary>
        /// Attempt to make a date separator token.
        /// </summary>
        /// <returns>true if the date separator token could be made, false if not.</returns>
        protected bool TryMakeSeparator()
        {
            if (_separator == Token.None)
            {
                return TryMakeToken(DateSeparators, out _separator);
            }

            return TryMakeToken(_separator);
        }

        /// <summary>
        /// Attempt to make the day part of the date.
        /// </summary>
        /// <param name="day">The day part of a date.</param>
        /// <returns>true if a day part could be made, false if not.</returns>
        protected bool TryMakeDayPart(out int day)
        {
            return TryMakeNumeric(1, 2, out day) && day > 0 && day <= 31;
        }

        /// <summary>
        /// Attempt to make the month part of the date.
        /// </summary>
        /// <param name="month">The month part of a date.</param>
        /// <returns>true if a month part could be made, false if not.</returns>
        protected bool TryMakeMonthPart(out int month)
        {
            if (TryMakeMonthPartNumeric(out month))
            {
                return true;
            }

            return TryMakeMonthPartText(out month);
        }

        /// <summary>
        /// Attempt to make the month part of the date.
        /// </summary>
        /// <param name="month">The month part of a date.</param>
        /// <returns>true if a month part could be made, false if not.</returns>
        protected bool TryMakeMonthPartNumeric(out int month)
        {
            return TryMakeNumeric(1, 2, out month) && month > 0 && month <= 12;
        }

        /// <summary>
        /// Attempt to make the month part of the date.
        /// </summary>
        /// <param name="month">The month part of a date.</param>
        /// <returns>true if a month part could be made, false if not.</returns>
        protected bool TryMakeMonthPartText(out int month)
        {
            month = 0;

            var token = Enumerator.Peek();
            if (token.Kind != TokenKind.Text)
            {
                return false;
            }

            var dictionary = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
            {
                { "Jan", 1 },
                { "Feb", 2 },
                { "Mar", 3 },
                { "Apr", 4 },
                { "May", 5 },
                { "Jun", 6 },
                { "Jul", 7 },
                { "Aug", 8 },
                { "Sep", 9 },
                { "Oct", 10 },
                { "Nov", 11 },
                { "Dec", 12 },
                { "January", 1 },
                { "February", 2 },
                { "March", 3 },
                { "April", 4 },
                { "June", 6 },
                { "July", 7 },
                { "August", 8 },
                { "September", 9 },
                { "October", 10 },
                { "November", 11 },
                { "December", 12 }
            };

            if (dictionary.TryGetValue(token.Text, out month))
            {
                Enumerator.Take();
                return month > 0 && month <= 12;
            }

            return false;
        }
        
        /// <summary>
        /// Attempt to make the year part of the date.
        /// </summary>
        /// <param name="year">The year part of a date.</param>
        /// <returns>true if a year part could be made, false if not.</returns>
        protected bool TryMakeYearPart(out int year)
        {
            return TryMake2DigitYearPart(out year) || TryMake4DigitYearPart(out year);
        }

        /// <summary>
        /// Attempt to make the year part of the date.
        /// </summary>
        /// <param name="year">The year part of a date.</param>
        /// <returns>true if a year part could be made, false if not.</returns>
        protected bool TryMake2DigitYearPart(out int year)
        {
            return TryMakeNumeric(2, out year) && year >= 0 && year <= 9999;
        }

        /// <summary>
        /// Attempt to make the year part of the date.
        /// </summary>
        /// <param name="year">The year part of a date.</param>
        /// <returns>true if a year part could be made, false if not.</returns>
        protected bool TryMake4DigitYearPart(out int year)
        {
            return TryMakeNumeric(4, out year) && year >= 0 && year <= 9999;
        }
    }
}
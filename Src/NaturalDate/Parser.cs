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
        /// Attempt to make a date.
        /// </summary>
        /// <param name="builder">The date builder that was used to build the date.</param>
        /// <returns>true if a date could be made, false if not.</returns>
        internal bool TryMakeDate(IDateBuilder builder)
        {
            if (new DayFirstDateParser(_enumerator).TryMakeDate(builder))
            {
                return true;
            }

            if (new MonthFirstDateParser(_enumerator).TryMakeDate(builder))
            {
                return true;
            }

            if (new YearFirstDateParser(_enumerator).TryMakeDate(builder))
            {
                return true;
            }

            return false;
        }
    }
}

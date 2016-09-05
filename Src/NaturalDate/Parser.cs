using NaturalDate.Text;

namespace NaturalDate
{
    public abstract class Parser
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="enumerator">The token enumerator to handle the incoming tokens.</param>
        protected Parser(TokenEnumerator enumerator)
        {
            Enumerator = enumerator;
        }

        /// <summary>
        /// Delegate for the TryMake function to allow for "out" parameters.
        /// </summary>
        /// <typeparam name="T">The type of the out parameter.</typeparam>
        /// <param name="found">The out parameter that was found during the make operation.</param>
        /// <returns>true if the make operation found a parameter, false if not.</returns>
        protected delegate bool TryMakeDelegate<T>(out T found);

        /// <summary>
        /// Try to match a function on the enumerator ensuring that the enumerator is restored on failure.
        /// </summary>
        /// <param name="make">The callback function to match on the enumerator.</param>
        /// <param name="found">The out value that was found.</param>
        /// <returns>true if the matching function successfully made a match, false if not.</returns>
        protected bool TryMake<T>(TryMakeDelegate<T> make, out T found)
        {
            using (var checkpoint = Enumerator.Checkpoint())
            {
                if (make(out found))
                {
                    return true;
                }

                checkpoint.Rollback();
            }

            return false;
        }

        /// <summary>
        /// Returns the enumerator to handle the incoming tokens.
        /// </summary>
        protected TokenEnumerator Enumerator { get; }
    }
}
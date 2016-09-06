namespace NaturalDate
{
    public interface IDateTimeBuilder<out T> : IDateTimeBuilder
    {
        /// <summary>
        /// Returns the date that has been built.
        /// </summary>
        /// <returns>The date that has been built.</returns>
        T Build();
    }

    public interface IDateTimeBuilder : IDateBuilder, ITimeBuilder { }
}
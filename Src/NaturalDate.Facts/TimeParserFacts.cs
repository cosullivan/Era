using System;
using System.Collections.Generic;
using NaturalDate.Text;
using Xunit;

namespace NaturalDate.Facts
{
    public sealed class TimeParserFacts
    {
        static readonly DateTime Reference = new DateTime(1978, 9, 10, 14, 30, 10);

        [Theory]
        [MemberData(nameof(TheoryValues))]
        public void Parse_Time_Success(string text, DateTime expected)
        {
            // arrange
            var parser = new TimeParser(new TokenEnumerator(new StringTokenReader(text)));

            // act
            var builder = new DateTimeBuilder(Reference);
            var result = parser.TryMake(builder);

            // assert
            Assert.True(result);
            Assert.Equal(expected, builder.Build());
        }

        public static IEnumerable<object[]> TheoryValues
        {
            get
            {
                return new List<object[]>
                {
                    new object[] { "5", new DateTime(Reference.Year, Reference.Month, Reference.Day, 5, 0, 0) },
                    new object[] { "18", new DateTime(Reference.Year, Reference.Month, Reference.Day, 18, 0, 0) },
                    new object[] { "10:32", new DateTime(Reference.Year, Reference.Month, Reference.Day, 10, 32, 0) },
                    new object[] { "10:32:45", new DateTime(Reference.Year, Reference.Month, Reference.Day, 10, 32, 45) },
                    new object[] { "10 AM", new DateTime(Reference.Year, Reference.Month, Reference.Day, 10, 0, 0) },
                    new object[] { "10 PM", new DateTime(Reference.Year, Reference.Month, Reference.Day, 22, 0, 0) },
                    new object[] { "10:20 PM", new DateTime(Reference.Year, Reference.Month, Reference.Day, 22, 20, 0) },
                    new object[] { "10:20:30 pm", new DateTime(Reference.Year, Reference.Month, Reference.Day, 22, 20, 30) },
                };
            }
        }
    }
}

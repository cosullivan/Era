using System;
using System.Collections.Generic;
using NaturalDate.Text;
using Xunit;

namespace NaturalDate.Facts
{
    public sealed class MonthFirstDateParserFacts
    {
        static readonly DateTime Reference = new DateTime(1978, 9, 10, 14, 30, 10);

        [Theory]
        [MemberData(nameof(TheoryValues))]
        public void Parse_CalendarDates_Success(string text, DateTime expected)
        {
            // arrange
            var parser = new MonthFirstDateParser(new TokenEnumerator(new StringTokenReader(text)));

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
                    new object[] { "Sep", new DateTime(1978, 9, 1, 0, 0, 0) },
                    new object[] { "September", new DateTime(1978, 9, 1, 0, 0, 0) },
                    new object[] { "Sep 2002", new DateTime(2002, 9, 1, 0, 0, 0) },
                    new object[] { "September 2002", new DateTime(2002, 9, 1, 0, 0, 0) },
                    new object[] { "Sep/2002", new DateTime(2002, 9, 1, 0, 0, 0) },
                    new object[] { "September/2002", new DateTime(2002, 9, 1, 0, 0, 0) },
                };
            }
        }
    }
}

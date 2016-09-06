using System;
using System.Collections.Generic;
using NaturalDate.Text;
using Xunit;

namespace NaturalDate.Facts
{
    public sealed class YearFirstDateParserFacts
    {
        static readonly DateTime Reference = new DateTime(1978, 9, 10, 14, 30, 10);

        [Theory]
        [MemberData(nameof(TheoryValues))]
        public void Parse_CalendarDates_Success(string text, DateTime expected)
        {
            // arrange
            var parser = new YearFirstDateParser(new TokenEnumerator(new StringTokenReader(text)));

            // act
            var builder = new DateBuilder(Reference);
            var result = parser.TryMakeDate(builder);

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
                    new object[] { "1978", new DateTime(1978, 1, 1, 0, 0, 0) },
                    new object[] { "1978-Sep", new DateTime(1978, 9, 1, 0, 0, 0) },
                    new object[] { "1978-Sep-10", new DateTime(1978, 9, 10, 0, 0, 0) },
                };
            }
        }
    }
}

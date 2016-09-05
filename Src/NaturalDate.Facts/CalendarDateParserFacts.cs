using System;
using System.Collections.Generic;
using NaturalDate.Text;
using Xunit;

namespace NaturalDate.Facts
{
    public sealed class CalendarDateParserFacts
    {
        static readonly DateTime Reference = new DateTime(1978, 9, 10, 14, 30, 10);

        [Theory]
        [MemberData(nameof(TheoryValues))]
        public void Parse_CalendarDates_Success(string text, DateTime expected)
        {
            // arrange
            var parser = new CalendarDateParser(new TokenEnumerator(new StringTokenReader(text)));

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
                    new object[] { "11/10/1978", new DateTime(1978, 10, 11, 0, 0, 0) },
                    new object[] { "11/10/78", new DateTime(2078, 10, 11, 0, 0, 0) },
                    new object[] { "11/Oct/78", new DateTime(2078, 10, 11, 0, 0, 0) },
                    new object[] { "11-Oct-78", new DateTime(2078, 10, 11, 0, 0, 0) },
                    new object[] { "11.Oct.78", new DateTime(2078, 10, 11, 0, 0, 0) },
                    new object[] { "10 Sep 1978", new DateTime(1978, 9, 10, 0, 0, 0) },
                    new object[] { "12", new DateTime(Reference.Year, Reference.Month, 12, 0, 0, 0) },
                    new object[] { "12/5", new DateTime(Reference.Year, 5, 12, 0, 0, 0) },
                    new object[] { "1978-Sep-10", new DateTime(1978, 9, 10, 0, 0, 0) },
                };
            }
        }
    }
}

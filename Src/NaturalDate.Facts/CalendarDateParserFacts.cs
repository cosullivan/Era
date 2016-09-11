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
                    new object[] { "11/10/1978", new DateTime(1978, 10, 11, 0, 0, 0) },
                    new object[] { "11/10/78", new DateTime(2078, 10, 11, 0, 0, 0) },
                    new object[] { "11/Oct/78", new DateTime(2078, 10, 11, 0, 0, 0) },
                    new object[] { "11-Oct-78", new DateTime(2078, 10, 11, 0, 0, 0) },
                    new object[] { "11.Oct.78", new DateTime(2078, 10, 11, 0, 0, 0) },
                    new object[] { "10 Sep 1978", new DateTime(1978, 9, 10, 0, 0, 0) },
                    new object[] { "10/sep/1978", new DateTime(1978, 9, 10, 0, 0, 0) },
                    new object[] { "12", new DateTime(Reference.Year, Reference.Month, 12, 0, 0, 0) },
                    new object[] { "12/5", new DateTime(Reference.Year, 5, 12, 0, 0, 0) },
                    new object[] { "Sep", new DateTime(1978, 9, 1, 0, 0, 0) },
                    new object[] { "September", new DateTime(1978, 9, 1, 0, 0, 0) },
                    new object[] { "Sep 2002", new DateTime(2002, 9, 1, 0, 0, 0) },
                    new object[] { "September 2002", new DateTime(2002, 9, 1, 0, 0, 0) },
                    new object[] { "Sep/2002", new DateTime(2002, 9, 1, 0, 0, 0) },
                    new object[] { "September/2002", new DateTime(2002, 9, 1, 0, 0, 0) },
                    new object[] { "1978", new DateTime(1978, 1, 1, 0, 0, 0) },
                    new object[] { "1978-Sep", new DateTime(1978, 9, 1, 0, 0, 0) },
                    new object[] { "1978-Sep-10", new DateTime(1978, 9, 10, 0, 0, 0) },
                    new object[] { "3:45:54 AM", new DateTime(Reference.Year, Reference.Month, Reference.Day, 3, 45, 54) },
                    new object[] { "3:45:54AM", new DateTime(Reference.Year, Reference.Month, Reference.Day, 3, 45, 54) },
                    new object[] { "3:45 AM", new DateTime(Reference.Year, Reference.Month, Reference.Day, 3, 45, 0) },
                    new object[] { "3:45AM", new DateTime(Reference.Year, Reference.Month, Reference.Day, 3, 45, 0) },
                    new object[] { "3 AM", new DateTime(Reference.Year, Reference.Month, Reference.Day, 3, 45, 0) },
                    new object[] { "23:45:54", new DateTime(Reference.Year, Reference.Month, Reference.Day, 23, 45, 54) },
                    new object[] { "23:45", new DateTime(Reference.Year, Reference.Month, Reference.Day, 23, 45, 0) },
                    new object[] { "3 AM", new DateTime(Reference.Year, Reference.Month, Reference.Day, 3, 0, 0) },
                    new object[] { "12 AM", new DateTime(Reference.Year, Reference.Month, Reference.Day, 0, 0, 0) },
                    new object[] { "12 PM", new DateTime(Reference.Year, Reference.Month, Reference.Day, 12, 0, 0) },
                };
            }
        }
    }
}

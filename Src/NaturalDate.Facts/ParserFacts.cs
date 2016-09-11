using System;
using System.Collections.Generic;
using NaturalDate.Text;
using Xunit;

namespace NaturalDate.Facts
{
    public sealed class ParserFacts
    {
        static readonly DateTime Reference = new DateTime(1978, 9, 10, 14, 30, 10); // Sunday

        [Theory]
        [MemberData(nameof(SuccessTheoryValues))]
        public void Parse_Success(string text, DateTime expected)
        {
            // arrange
            var parser = new Parser(new TokenEnumerator(new StringTokenReader(text)));

            // act
            var builder = new DateTimeBuilder(Reference);
            var result = parser.TryMake(builder);

            // assert
            Assert.True(result);
            Assert.Equal(expected, builder.Build());
        }

        [Theory]
        [MemberData(nameof(FailureTheoryValues))]
        public void Parse_Failure(string text)
        {
            // arrange
            var parser = new Parser(new TokenEnumerator(new StringTokenReader(text)));

            // act
            var builder = new DateTimeBuilder(Reference);
            var result = parser.TryMake(builder);

            // assert
            Assert.False(result);
        }

        public static IEnumerable<object[]> SuccessTheoryValues
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
                    new object[] { "3 AM", new DateTime(Reference.Year, Reference.Month, Reference.Day, 3, 0, 0) },
                    new object[] { "23:45:54", new DateTime(Reference.Year, Reference.Month, Reference.Day, 23, 45, 54) },
                    new object[] { "23:45", new DateTime(Reference.Year, Reference.Month, Reference.Day, 23, 45, 0) },
                    new object[] { "12 AM", new DateTime(Reference.Year, Reference.Month, Reference.Day, 0, 0, 0) },
                    new object[] { "12 PM", new DateTime(Reference.Year, Reference.Month, Reference.Day, 12, 0, 0) },
                    new object[] { "11/10/1978 3 PM", new DateTime(1978, 10, 11, 15, 0, 0) },
                    new object[] { "11/10/1978 3:45 PM", new DateTime(1978, 10, 11, 15, 45, 0) },
                    new object[] { "11/10/1978 3:45:54 PM", new DateTime(1978, 10, 11, 15, 45, 54) },
                    new object[] { "Sep 3 PM", new DateTime(1978, 9, 1, 15, 0, 0) },
                    new object[] { "now", Reference },
                    new object[] { "today", new DateTime(Reference.Year, Reference.Month, Reference.Day, 0, 0, 0) },
                    new object[] { "tomorrow", new DateTime(Reference.Year, Reference.Month, Reference.Day, 0, 0, 0).AddDays(1) },
                    new object[] { "yesterday", new DateTime(Reference.Year, Reference.Month, Reference.Day, 0, 0, 0).AddDays(-1) },
                    new object[] { "sunday", new DateTime(Reference.Year, Reference.Month, Reference.Day, 0, 0, 0).AddDays(7) },
                    new object[] { "monday", new DateTime(Reference.Year, Reference.Month, Reference.Day, 0, 0, 0).AddDays(1) },
                    new object[] { "tuesday", new DateTime(Reference.Year, Reference.Month, Reference.Day, 0, 0, 0).AddDays(2) },
                    new object[] { "wednesday", new DateTime(Reference.Year, Reference.Month, Reference.Day, 0, 0, 0).AddDays(3) },
                    new object[] { "thursday", new DateTime(Reference.Year, Reference.Month, Reference.Day, 0, 0, 0).AddDays(4) },
                    new object[] { "friday", new DateTime(Reference.Year, Reference.Month, Reference.Day, 0, 0, 0).AddDays(5) },
                    new object[] { "saturday", new DateTime(Reference.Year, Reference.Month, Reference.Day, 0, 0, 0).AddDays(6) },
                    new object[] { "monday 2am", new DateTime(Reference.Year, Reference.Month, Reference.Day, 2, 0, 0).AddDays(1) },
                };
            }
        }

        public static IEnumerable<object[]> FailureTheoryValues
        {
            get
            {
                return new List<object[]>
                {
                    new object[] { "131/10/1978" },
                    new object[] { "31/Feb/1978" },
                };
            }
        }
    }
}
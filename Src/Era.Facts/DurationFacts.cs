using System;
using System.Collections.Generic;
using Era.Text;
using Xunit;

namespace Era.Facts
{
    public sealed class DurationFacts
    {
        static readonly DateTime Reference = new DateTime(2015, 9, 10, 14, 30, 10); // Sunday

        [Theory]
        [MemberData(nameof(SuccessTheoryValues))]
        public void Parse_Success(string text, TimeSpan expected)
        {
            // arrange
            var parser = new DurationParser(new TokenEnumerator(new StringTokenReader(text)));

            // act
            var builder = new DurationBuilder(Reference);
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
            var parser = new DurationParser(new TokenEnumerator(new StringTokenReader(text)));

            // act
            var builder = new DurationBuilder(Reference);
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
                    new object[] { "2y", TimeSpan.FromDays(731) }, // reference time is a leap year
                    new object[] { "2h", TimeSpan.FromHours(2) },
                };
            }
        }

        public static IEnumerable<object[]> FailureTheoryValues
        {
            get
            {
                return new List<object[]>
                {
                    new object[] { "1s 2h" },
                    new object[] { "12.33.4m" },
                };
            }
        }
    }
}
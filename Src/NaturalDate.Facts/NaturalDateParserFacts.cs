//using System;
//using System.Collections.Generic;
//using NaturalDate.Text;
//using Xunit;

//namespace NaturalDate.Facts
//{
//    public sealed class NaturalDateParserFacts
//    {
//        static readonly DateTime Reference = new DateTime(2015, 9, 4, 14, 30, 10); // Friday

//        [Theory]
//        [MemberData(nameof(TheoryValues))]
//        public void Parse_NaturalDates_Success(string text, DateTime expected)
//        {
//            // arrange
//            var parser = new NaturalDateParser(new TokenEnumerator(new StringTokenReader(text)));

//            // act
//            var builder = new DateTimeBuilder(Reference);
//            var result = parser.TryMake(builder);

//            // assert
//            Assert.True(result);
//            Assert.Equal(expected, builder.Build());
//        }

//        public static IEnumerable<object[]> TheoryValues
//        {
//            get
//            {
//                return new List<object[]>
//                {
//                    new object[] { "now", Reference },
//                    new object[] { "today", new DateTime(Reference.Year, Reference.Month, Reference.Day, 0, 0, 0) },
//                    new object[] { "tomorrow", new DateTime(Reference.Year, Reference.Month, Reference.Day, 0, 0, 0).AddDays(1) },
//                    new object[] { "monday", new DateTime(Reference.Year, Reference.Month, Reference.Day, 0, 0, 0).AddDays(3) },
//                    new object[] { "friday", new DateTime(Reference.Year, Reference.Month, Reference.Day, 0, 0, 0).AddDays(7) }
//                };
//            }
//        }
//    }
//}

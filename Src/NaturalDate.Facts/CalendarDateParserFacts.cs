//using System;
//using System.Collections.Generic;
//using System.Linq;
//using NaturalDate.Text;
//using Xunit;

//namespace NaturalDate.Facts
//{
//    public sealed class CalendarDateParserFacts
//    {
//        static readonly DateTime Reference = new DateTime(1978, 9, 10, 14, 30, 10);

//        [Theory]
//        [MemberData(nameof(TheoryValues))]
//        public void Parse_CalendarDates_Success(string text, DateTime expected)
//        {
//            // arrange
//            var parser = new CalendarDateParser(new TokenEnumerator(new StringTokenReader(text)));

//            // act
//            var builder = new DateBuilder(Reference);
//            var result = parser.TryMakeDate(builder);

//            // assert
//            Assert.True(result);
//            Assert.Equal(expected, builder.Build());
//        }

//        public static IEnumerable<object[]> TheoryValues
//        {
//            get { return DayFirstDateParserFacts.TheoryValues.Union(YearFirstDateParserFacts.TheoryValues).Union(MonthFirstDateParserFacts.TheoryValues).ToList(); }
//        }
//    }
//}

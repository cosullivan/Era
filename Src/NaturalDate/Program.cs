using System;
using NaturalDate.Text;

namespace NaturalDate
{
    class Program
    {
        static void Main(string[] args)
        {
            //var time = "31/sep/19";
            //var time = "now";

            //var reader = new StringTokenReader(time);
            //Token token;
            //while ((token = reader.NextToken()) != Token.None)
            //{
            //    Console.WriteLine(token);
            //}

            //var parser = new DateParser(new StringTokenReader(time));

            //DateTime date;
            //Console.WriteLine(parser.TryMakeDate(out date));
            //Console.WriteLine(date);

            var time = "12";

            var parser = new TimeParser(new TokenEnumerator(new StringTokenReader(time)));

            DateTime date;
            Console.WriteLine(parser.TryMakeTime(out date));
            Console.WriteLine(date);
        }
    }
}

﻿using System;
using System.Diagnostics;
using System.Text;
using NaturalDate;
using NaturalDate.Text;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine(DateTime.Parse("23 Sep 2014 23:45"));
            //Chronic.Parser p = new Chronic.Parser();
            //var span = p.Parse("24/9/2015 156");
            //Console.WriteLine(span);
            //return;

            //var input = "30/9/1999";
            //var input = "21:12:25";
            var input = "11/10/1978 3:45:54 AM";
            var parser = new CalendarDateParser(new TokenEnumerator(new StringTokenReader(input)));

            var builder = new DateTimeBuilder(DateTime.Now);
            Console.WriteLine(parser.TryMake(builder));
            Console.WriteLine(builder.Build());

            //return;

            //DateTime date;
            //Parser.TryParse("1/Feb/1978 10 am", out date);
            //Console.WriteLine(date);

            //Parser.TryParse("1", out date);
            //Console.WriteLine(date);

            //Parser.TryParse("1.May", out date);
            //Console.WriteLine(date);

            //Parser.TryParse("May 1978", out date);
            //Console.WriteLine(date);

            //Parser.TryParse("now", out date);
            //Console.WriteLine(date);

            //Parser.TryParse("Monday", out date);
            //Console.WriteLine(date);

            //Parser.TryParse("Tuesday", out date);
            //Console.WriteLine(date);

            //Parser.TryParse("Wednesday", out date);
            //Console.WriteLine(date);

            //Parser.TryParse("Thursday", out date);
            //Console.WriteLine(date);

            //5445

            //RunBenchmarks();
        }

        static void RunBenchmarks()
        {
            Console.WriteLine("System.DateTime");
            Console.WriteLine("  Time Taken {0}ms", RunBenchmark(DateTimeParse));
            Console.WriteLine("  Time Taken {0}ms", RunBenchmark(DateTimeParse));
            Console.WriteLine("  Time Taken {0}ms", RunBenchmark(DateTimeParse));

            Console.WriteLine();

            Console.WriteLine("NaturalDate");
            Console.WriteLine("  Time Taken {0}ms", RunBenchmark(ParserTryParse));
            Console.WriteLine("  Time Taken {0}ms", RunBenchmark(ParserTryParse));
            Console.WriteLine("  Time Taken {0}ms", RunBenchmark(ParserTryParse));

            Console.WriteLine();

            Console.WriteLine("Chronic");
            Console.WriteLine("  Time Taken {0}ms", RunBenchmark(ChronicParse));
            Console.WriteLine("  Time Taken {0}ms", RunBenchmark(ChronicParse));
            Console.WriteLine("  Time Taken {0}ms", RunBenchmark(ChronicParse));
        }

        static long RunBenchmark(Action<int> callback, int iterations = 1000)
        {
            var stopwatch = Stopwatch.StartNew();

            callback(iterations);

            stopwatch.Stop();

            return stopwatch.ElapsedMilliseconds;
        }

        static void DateTimeParse(int iterations)
        {
            for (var i = 0; i < iterations; i++)
            {
                var date = DateTime.Parse("11/10/1978 3:45:54 AM");
            }
        }

        static void ParserTryParse(int iterations)
        {
            for (var i = 0; i < iterations; i++)
            {
                DateTime date;
                Parser.TryParse("11/10/1978 3:45:54 AM", out date);
            }
        }

        static void ChronicParse(int iterations)
        {
            for (var i = 0; i < iterations; i++)
            {
                Chronic.Parser p = new Chronic.Parser();
                p.Parse("11/10/1978 3:45:54 AM");
            }
        }
    }
}
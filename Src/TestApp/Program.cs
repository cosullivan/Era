﻿using System;
using System.Diagnostics;
using System.Text;
using NaturalDate;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime date;
            Parser.TryParse("1/Feb/1978 10 am", out date);
            Console.WriteLine(date);

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

            //var stopwatch = Stopwatch.StartNew();
            //DateTimeParse(1000);
            //stopwatch.Stop();
            //Console.WriteLine("Time Taken {0}ms", stopwatch.ElapsedMilliseconds);

            //stopwatch = Stopwatch.StartNew();
            //DateTimeParse(1000);
            //stopwatch.Stop();
            //Console.WriteLine("Time Taken {0}ms", stopwatch.ElapsedMilliseconds);

            //stopwatch = Stopwatch.StartNew();
            //DateTimeParse(1000);
            //stopwatch.Stop();
            //Console.WriteLine("Time Taken {0}ms", stopwatch.ElapsedMilliseconds);
        }

        //static void DateTimeParse(int iterations)
        //{
        //    for (var i = 0; i < iterations; i++)
        //    {
        //        var date = DateTime.Parse("10/09/1978");
        //    }
        //}

        //static void DateTimeParse(int iterations)
        //{
        //    for (var i = 0; i < iterations; i++)
        //    {
        //        DateTime date;
        //        Parser.TryParse("10/09/1978", out date);
        //    }
        //}

        //static void DateTimeParse(int iterations)
        //{
        //    for (var i = 0; i < iterations; i++)
        //    {
        //        Chronic.Parser p = new Chronic.Parser();
        //        p.Parse("10/09/1978");
        //    }
        //}
    }
}
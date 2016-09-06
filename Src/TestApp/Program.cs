using System;
using System.Diagnostics;
using System.Text;
using NaturalDate;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //DateTime date;
            //Parser.TryParse("Feb", out date);
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

            //var stopwatch = Stopwatch.StartNew();
            //DateTimeParse(1000000);
            //stopwatch.Stop();
            //Console.WriteLine("Time Taken {0}ms", stopwatch.ElapsedMilliseconds);

            //stopwatch = Stopwatch.StartNew();
            //DateTimeParse(1000000);
            //stopwatch.Stop();
            //Console.WriteLine("Time Taken {0}ms", stopwatch.ElapsedMilliseconds);

            //stopwatch = Stopwatch.StartNew();
            //DateTimeParse(1000000);
            //stopwatch.Stop();
            //Console.WriteLine("Time Taken {0}ms", stopwatch.ElapsedMilliseconds);

            Console.SetCursorPosition(0, 3);

            StringBuilder input = new StringBuilder();
            while (true)
            {
                var ch = Console.ReadKey();

                if (ch.Key == ConsoleKey.Backspace && input.Length > 0)
                {
                    input = input.Remove(input.Length - 1, 1);
                    continue;
                }

                input.Append(ch.Key);

                DateTime date;
                if (Parser.TryParse(input.ToString(), out date))
                {
                    var left = Console.CursorLeft;
                    Console.SetCursorPosition(0, 0);
                    Console.Write(date);
                    Console.SetCursorPosition(left, 3);
                }
            }
        }

        //static void DateTimeParse(int iterations)
        //{
        //    for (var i = 0; i < iterations; i++)
        //    {
        //        var date = DateTime.Parse("10/09/1978");
        //    }
        //}

        static void DateTimeParse(int iterations)
        {
            for (var i = 0; i < iterations; i++)
            {
                DateTime date;
                Parser.TryParse("10/09/1978", out date);
            }
        }
    }
}
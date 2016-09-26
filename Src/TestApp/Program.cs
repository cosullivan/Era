using System;
using System.Diagnostics;
using Era;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //var input = "tomorrow";
            //DateTime date;
            //Console.WriteLine(Parser.TryParse(input, out date));
            //Console.WriteLine(date);

            RunBenchmarks();
        }

        static void RunBenchmarks()
        {
            RunBenchmark("System.DateTime", DateTimeParse);
            RunBenchmark("Era", EraTryParse);
            RunBenchmark("Chronic", ChronicParse);
        }

        static void RunBenchmark(string name, Action<int> callback, int iterations = 1000)
        {
            Console.WriteLine();
            Console.WriteLine(name);

            // ignore the first call
            RunBenchmark(callback);

            decimal total = 0;
            for (var i = 0; i < 5; i++)
            {
                var benchmark = RunBenchmark(callback);

                Console.WriteLine("  Time Taken {0}ms", benchmark);

                total += benchmark;
            }

            Console.WriteLine("  Avg Time {0}ms", Decimal.Round(total / 5, 2));
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

        static void EraTryParse(int iterations)
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
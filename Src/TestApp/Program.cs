using System;
using Era;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            ////var input = "tomorrow";
            ////DateTime date;
            ////Console.WriteLine(Parser.TryParse(input, out date));
            ////Console.WriteLine(date);
            //var bits = Decimal.GetBits(1.369m);

            //Console.WriteLine(bits[0]);
            //Console.WriteLine(bits[1]);
            //Console.WriteLine(bits[2]);
            //Console.WriteLine((bits[3] >> 16) & 31);

            //var n = 369;
            //Console.WriteLine(Math.Log(n));
            //Console.WriteLine(Math.Log10(n));
            //Console.WriteLine(Math.Exp(n));
            //Console.WriteLine(Math.Floor(Math.Log10(n) + 1));

            ////var d = 0.0;
            ////d = (d / 10.0) + (9 / 10.0);
            ////d = (d / 10.0) + (6 / 10.0);
            ////d = (d / 10.0) + (3 / 10.0);

            //var d = 0.0;
            //d = (d / 1.0) + (3 / 10.0);
            //d = (d / 1.0) + (6 / 100.0);
            //d = (d / 1.0) + (9 / 1000.0);

            //Console.WriteLine(Math.Pow(10, 3));

            //Console.WriteLine(d);

            //var input = "1.5y 2.6mo 3d 4h 5m 6s";
            var input = "1.5y 2.6mo";
            TimeSpan time;
            Console.WriteLine(Parser.TryParse(input, out time));
            //Console.WriteLine($"{time.Year} {time.Month} {time.Day} {time.Hour} {time.Minute} {time.Second}");
            Console.WriteLine($"{time}");
        }
    }
}
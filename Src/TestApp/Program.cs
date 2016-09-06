using System;
using NaturalDate;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime date;
            Console.WriteLine(Parser.TryParse("Feb", out date));
            Console.WriteLine(date);

            Console.WriteLine(Parser.TryParse("1", out date));
            Console.WriteLine(date);

            Console.WriteLine(Parser.TryParse("1.May", out date));
            Console.WriteLine(date);
        }
    }
}

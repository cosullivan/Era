using System;
using Era;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = "tomorrow";
            DateTime date;
            Console.WriteLine(Parser.TryParse(input, out date));
            Console.WriteLine(date);
        }
    }
}
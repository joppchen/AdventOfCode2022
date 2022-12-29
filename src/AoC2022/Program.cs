using System;

namespace AoC2022
{
    internal static class Program
    {
        private const int Year = 2022;

        private static void Main(string[] args)
        {
            Console.WriteLine($"Hello Advent of Code {Year}!");

            var dayNumber = 25; // Current default

            if (args.Length > 0) dayNumber = int.Parse(args[0]);
            Console.WriteLine($"Day to be calculated: Day {dayNumber}.");
            Console.WriteLine("");

            Day25.Main.Solve(dayNumber);
        }
    }
}
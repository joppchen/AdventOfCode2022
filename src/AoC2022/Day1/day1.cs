using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using AoC2022.Common;

namespace AoC2022.Day1
{
    internal static class Main
    {
        public static void Solve(int dayNumber)
        {
            Console.WriteLine($"Day 1:");
            const string filename = "day1";

            var textFile = $"../../../../../resources/{filename}.txt";
            //var textFile = $"../../../../../resources/{filename}_example.txt";

            if (File.Exists(textFile))
            {
                var lines = File.ReadLines(textFile).ToList();
                var sums = new List<int>();
                var sum = 0;
                foreach (var line in lines)
                {
                    if (line.Equals(""))
                    {
                        sums.Add(sum);
                        sum = 0;
                    }
                    else
                    {
                        sum += int.Parse(line);
                        if (line.Equals(lines.Last()))
                        {
                            sums.Add(sum);
                        }
                    }
                }

                var maxSum = sums.Max();

                Console.WriteLine("TASK 1");
                var watch = System.Diagnostics.Stopwatch.StartNew();
                var result = maxSum; // Answer: 71300
                watch.Stop();
                Console.WriteLine($"Task 1: {result}. Elapsed time [ms]: {watch.ElapsedMilliseconds}");

                Console.WriteLine("");
                Console.WriteLine("TASK 2");
                watch = System.Diagnostics.Stopwatch.StartNew();
                result = sums.OrderByDescending(x => x).Take(3).Sum(); // Answer: 209691
                watch.Stop();
                Console.WriteLine($"Task 2: {result}. Elapsed time [ms]: {watch.ElapsedMilliseconds}");
            }
            else
            {
                Console.WriteLine($"File not found: '{textFile}'");
            }
        }
    }
}
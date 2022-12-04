using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC2022.Day4
{
    internal static class Main
    {
        public static void Solve(int dayNumber)
        {
            Console.WriteLine($"Day {dayNumber}:");

            var filename = "day" + Convert.ToString(dayNumber);

            var textFile = $"../../../../../resources/{filename}.txt";
            // var textFile = $"../../../../../resources/{filename}_example.txt";

            if (File.Exists(textFile))
            {
                var items = File.ReadLines(textFile).ToList();

                // Task 1
                var areFullyContained = 0;
                foreach (var item in items)
                {
                    var assignments = item.Split(',');
                    var firstIsFullyContained = IsFullyContained(assignments.ToList(), false);
                    var lastIsFullyContained = IsFullyContained(assignments.ToList(), true);
                    if (firstIsFullyContained || lastIsFullyContained)
                    {
                        areFullyContained += 1;
                    }
                }

                Console.WriteLine("TASK 1");
                var watch = System.Diagnostics.Stopwatch.StartNew();
                var result = areFullyContained; // Answer: 441
                watch.Stop();
                Console.WriteLine($"Task 1: {result}. Elapsed time [ms]: {watch.ElapsedMilliseconds}");

                Console.WriteLine("");
                Console.WriteLine("TASK 2");
                watch = System.Diagnostics.Stopwatch.StartNew();

                var overLapping = items.Select(item => item.Split(',')).Count(Overlaps);

                result = overLapping; // Answer: 861
                watch.Stop();
                Console.WriteLine($"Task 2: {result}. Elapsed time [ms]: {watch.ElapsedMilliseconds}");
            }
            else
            {
                Console.WriteLine($"File not found: '{textFile}'");
            }
        }

        private static bool IsFullyContained(List<string> assignments, bool reverse)
        {
            if (reverse) assignments.Reverse();
            var range1 = assignments[0].Split('-');
            var range2 = assignments[1].Split('-');
            return int.Parse(range1[0]) <= int.Parse(range2[0]) &&
                   int.Parse(range1[1]) >= int.Parse(range2[1]);
        }

        private static bool Overlaps(IReadOnlyList<string> assignments)
        {
            var limitsA = assignments[0].Split('-').Select(int.Parse).ToArray();
            var limitsB = assignments[1].Split('-').Select(int.Parse).ToArray();

            var rangeA = Enumerable.Range(limitsA.First(), RangeCount(limitsA)).ToArray();
            var rangeB = Enumerable.Range(limitsB.First(), RangeCount(limitsB)).ToArray();

            return rangeA.Any(i => rangeB.Contains(i));
        }

        private static int RangeCount(int[] range)
        {
            return range.Last() - range.First() + 1;
        }
    }
}
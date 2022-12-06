using System;
using System.IO;
using System.Linq;
using AoC2022.Common;

namespace AoC2022.Day6
{
    internal static class Main
    {
        public static void Solve(int dayNumber)
        {
            Console.WriteLine($"Day {dayNumber}:");

            var filename = "day" + Convert.ToString(dayNumber);

            var textFile = $"../../../../../resources/{filename}.txt";
            //var textFile = $"../../../../../resources/{filename}_example5.txt";

            if (File.Exists(textFile))
            {
                var stream = File.ReadAllText(textFile).ToCharArray();

                // Task 1
                Console.WriteLine("TASK 1");
                var watch = System.Diagnostics.Stopwatch.StartNew();
                var result = StartOfPacketMarker(stream); // Answer: 1275
                watch.Stop();
                Console.WriteLine($"Task 1: {result}. Elapsed time [ms]: {watch.ElapsedMilliseconds}");

                Console.WriteLine("");
                Console.WriteLine("TASK 2");
                watch = System.Diagnostics.Stopwatch.StartNew();

                result = StartOfMessageMarker(stream); // Answer: 3605
                watch.Stop();
                Console.WriteLine($"Task 2: {result}. Elapsed time [ms]: {watch.ElapsedMilliseconds}");
            }
            else
            {
                Console.WriteLine($"File not found: '{textFile}'");
            }
        }

        private static int StartOfPacketMarker(char[] stream)
        {
            return StartOfMarker(stream, 4);
        }

        private static int StartOfMessageMarker(char[] stream)
        {
            return StartOfMarker(stream, 14);
        }

        private static int StartOfMarker(char[] stream, int markerLength)
        {
            for (var i = 0; i < stream.Length - markerLength; i++)
            {
                if (!AllUniqueCharacters(stream.RangeSubset(i, markerLength))) continue;
                return i + markerLength;
            }

            throw new Exception("Start of Packet Marker was not found! Check your input.");
        }

        private static bool AllUniqueCharacters(char[] range)
        {
            if (range.Length < 2) return true;
            var remainder = range.RangeSubset(1, range.Length - 1);
            return !remainder.Contains(range[0]) && AllUniqueCharacters(remainder);
        }
    }
}
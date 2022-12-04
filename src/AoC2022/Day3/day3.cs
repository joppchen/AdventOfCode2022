using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC2022.Day3
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
                var sharedItems = new List<char>();
                foreach (var item in items)
                {
                    var firstHalf = item.Substring(0, item.Length / 2);
                    var secondHalf = item.Substring(item.Length / 2);

                    foreach (var letter in firstHalf)
                    {
                        if (secondHalf.Contains(letter))
                        {
                            sharedItems.Add(letter);
                            break;
                        }
                    }
                }

                const int uCaseConverter = -65 + 27; // A = 65
                const int lCaseConverter = -97 + 1; // a = 97

                var priorities = (from item in sharedItems
                    let converter = char.IsLower(item) ? lCaseConverter : uCaseConverter
                    select (int) item + converter).ToList();

                Console.WriteLine("TASK 1");
                var watch = System.Diagnostics.Stopwatch.StartNew();
                var result = priorities.Sum(); // Answer: 7785
                watch.Stop();
                Console.WriteLine($"Task 1: {result}. Elapsed time [ms]: {watch.ElapsedMilliseconds}");

                Console.WriteLine("");
                Console.WriteLine("TASK 2");
                watch = System.Diagnostics.Stopwatch.StartNew();

                // TASK2
                sharedItems.Clear();
                foreach (var number in EveryNth(items, 3))
                {
                    var groupRucksacks = items.GetRange(number, 3);

                    foreach (var letter in groupRucksacks[0])
                    {
                        if (groupRucksacks[1].Contains(letter) && groupRucksacks[2].Contains(letter))
                        {
                            sharedItems.Add(letter);
                            break;
                        }
                    }
                }

                priorities.Clear();
                priorities = (from item in sharedItems
                    let converter = char.IsLower(item) ? lCaseConverter : uCaseConverter
                    select (int) item + converter).ToList();
                result = priorities.Sum(); // Answer: 2633
                watch.Stop();
                Console.WriteLine($"Task 2: {result}. Elapsed time [ms]: {watch.ElapsedMilliseconds}");
            }
            else
            {
                Console.WriteLine($"File not found: '{textFile}'");
            }
        }

        private static IEnumerable<int> EveryNth<T>(IReadOnlyCollection<T> listToJump, int n)
        {
            // Yield every Nth number in the list
            for (var i = 0; i < listToJump.Count(); i++)
            {
                if (i % 3 == 0)
                {
                    yield return i;
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC2022.Day2
{
    internal static class Main
    {
        private enum Shapes
        {
            Rock = 1,
            Paper = 2,
            Scissor = 3
        }

        private enum Outcome
        {
            Lose = 0,
            Draw = 3,
            Win = 6
        }

        public static void Solve(int dayNumber)
        {
            Console.WriteLine($"Day {dayNumber}:");

            var filename = "day" + Convert.ToString(dayNumber);

            var textFile = $"../../../../../resources/{filename}.txt";
            //var textFile = $"../../../../../resources/{filename}_example.txt";

            if (File.Exists(textFile))
            {
                var rounds = File.ReadLines(textFile).ToList();

                var elfCodesDict = new Dictionary<string, Shapes>
                    {{"A", Shapes.Rock}, {"B", Shapes.Paper}, {"C", Shapes.Scissor}};
                var myCodesDict = new Dictionary<string, Shapes>
                    {{"X", Shapes.Rock}, {"Y", Shapes.Paper}, {"Z", Shapes.Scissor}};

                var myPoints = 0;
                foreach (var round in rounds)
                {
                    var shapesInRound = round.Split(' ');
                    var elfShapeCode = shapesInRound[0];
                    var myShapeCode = shapesInRound[1];
                    myPoints += (int) myCodesDict[myShapeCode];
                    myPoints += (int) EvaluateGameOutcome(myCodesDict[myShapeCode], elfCodesDict[elfShapeCode]);
                }

                Console.WriteLine("TASK 1");
                var watch = System.Diagnostics.Stopwatch.StartNew();
                var result = myPoints; // Answer: 14531
                watch.Stop();
                Console.WriteLine($"Task 1: {result}. Elapsed time [ms]: {watch.ElapsedMilliseconds}");

                // Console.WriteLine("");
                // Console.WriteLine("TASK 2");
                // watch = System.Diagnostics.Stopwatch.StartNew();
                // result = sums.OrderByDescending(x => x).Take(3).Sum(); // Answer: 209691
                // watch.Stop();
                // Console.WriteLine($"Task 2: {result}. Elapsed time [ms]: {watch.ElapsedMilliseconds}");
            }
            else
            {
                Console.WriteLine($"File not found: '{textFile}'");
            }
        }

        private static Outcome EvaluateGameOutcome(Shapes myShape, Shapes elfShape)
        {
            var diff = (int) myShape - (int) elfShape;
            switch (diff)
            {
                case 0:
                    return Outcome.Draw;
                case 1:
                case -2:
                    return Outcome.Win;
                default:
                    return Outcome.Lose;
            }
        }
    }
}
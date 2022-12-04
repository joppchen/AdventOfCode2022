using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static AoC2022.Common.SharedMethods;

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

                // Task 1
                var myPoints = 0;
                foreach (var round in rounds)
                {
                    var shapesInRound = round.Split(' ');
                    var elfShapeCode = shapesInRound[0];
                    var myShapeCode = shapesInRound[1];
                    myPoints = AddPointForRound(myPoints, myCodesDict[myShapeCode], elfCodesDict[elfShapeCode]);
                }

                Console.WriteLine("TASK 1");
                var watch = System.Diagnostics.Stopwatch.StartNew();
                var result = myPoints; // Answer: 14531
                watch.Stop();
                Console.WriteLine($"Task 1: {result}. Elapsed time [ms]: {watch.ElapsedMilliseconds}");


                Console.WriteLine("");
                Console.WriteLine("TASK 2");
                watch = System.Diagnostics.Stopwatch.StartNew();

                var goalCodes = new Dictionary<string, Outcome>
                    {{"X", Outcome.Lose}, {"Y", Outcome.Draw}, {"Z", Outcome.Win}};

                myPoints = 0;
                var goalToInt = new Dictionary<Outcome, int> {{Outcome.Draw, 0}, {Outcome.Win, 1}, {Outcome.Lose, 2}};
                foreach (var round in rounds)
                {
                    var shapesInRound = round.Split(' ');
                    Shapes elfShape = elfCodesDict[shapesInRound[0]];

                    Outcome goalOfRound = goalCodes[shapesInRound[1]];
                    Shapes myChoice = ChooseCorrectShape(goalToInt, goalOfRound, elfShape);

                    myPoints = AddPointForRound(myPoints, myChoice, elfShape);
                }

                result = myPoints; // Answer: 11258
                watch.Stop();
                Console.WriteLine($"Task 2: {result}. Elapsed time [ms]: {watch.ElapsedMilliseconds}");
            }
            else
            {
                Console.WriteLine($"File not found: '{textFile}'");
            }
        }

        private static int AddPointForRound(int myPoints, Shapes myShape, Shapes elfShape)
        {
            myPoints += (int) myShape;
            myPoints += (int) EvaluateGameOutcome(myShape, elfShape);
            return myPoints;
        }

        private static Shapes ChooseCorrectShape(IReadOnlyDictionary<Outcome, int> goalToInt, Outcome goalOfRound,
            Shapes elfShape)
        {
            var myShapeIndex = (int) elfShape + goalToInt[goalOfRound];
            return (Shapes) Wrap(myShapeIndex - 1, goalToInt.Count) + 1; // A bit messy since my enum is 1-based
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
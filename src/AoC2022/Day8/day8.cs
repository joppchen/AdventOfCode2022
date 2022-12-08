using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC2022.Day8
{
    internal static class Main
    {
        public static void Solve(int dayNumber)
        {
            Console.WriteLine($"Day {dayNumber}:");

            var filename = "day" + Convert.ToString(dayNumber);

            // var textFile = $"../../../../../resources/{filename}.txt";
            var textFile = $"../../../../../resources/{filename}_example.txt";

            if (File.Exists(textFile))
            {
                var treeLines = File.ReadLines(textFile).ToList();
                var forestOld = new List<List<int>>();
                var rows = treeLines.Count;
                var cols = treeLines[0].Length;
                var forest = new int[rows, cols];

                for (var i = 0; i < rows; i++)
                {
                    var treeChars = treeLines[i].ToCharArray();
                    var treeInts = Array.ConvertAll(treeChars, c => (int) Char.GetNumericValue(c)).ToList();
                    for (var j = 0; j < cols; j++)
                    {
                        forest[i, j] = treeInts[j];
                    }
                }

                // var svada = IsVisibleInLineFromOneSide(1, new int[] {2, 5, 5, 1, 2});
                // var svadaReverse = IsVisibleInLineFromOneSide(rows - 1 - 1, new int[] {2, 1, 5, 5, 2});
                // var test3 = MapAllVisibleTreesOnLine(new int[]
                // {
                //     0, 1, 2, 1, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 2, 1, 3, 2, 3, 1, 2, 2, 3, 4, 1, 0, 4, 3, 0, 4, 2, 3, 1,
                //     3, 1, 3, 2, 5, 5, 1, 3, 4, 1, 3, 1, 2, 4, 2, 5, 4, 1, 4, 5, 5, 2, 3, 5, 5, 2, 3, 1, 4, 5, 2, 3, 2,
                //     4, 2, 2, 0, 2, 1, 4, 1, 3, 2, 3, 2, 3, 0, 2, 3, 2, 3, 1, 2, 1, 3, 2, 0, 0, 3, 2, 0, 1, 1, 1, 0, 0
                // }); // Denne stemmer
                // var sum3 = test3.Count(i => i);
                // var test3Reverse = MapAllVisibleTreesOnLine(new int[]
                // {
                //     0, 0, 1, 1, 1, 0, 2, 3, 0, 0, 2, 3, 1, 2, 1, 3, 2, 3, 2, 0, 3, 2, 3, 2, 3, 1, 4, 1, 2, 0, 2, 2, 4,
                //     2, 3, 2, 5, 4, 1, 3, 2, 5, 5, 3, 2, 5, 5, 4, 1, 4, 5, 2, 4, 2, 1, 3, 1, 4, 3, 1, 5, 5, 2, 3, 1, 3,
                //     1, 3, 2, 4, 0, 3, 4, 0, 1, 4, 3, 2, 2, 1, 3, 2, 3, 1, 2, 2, 2, 3, 3, 3, 1, 1, 1, 1, 1, 1, 2, 1, 0
                // });
                // var sum3reverse = test3Reverse.Count(i => i);


                var visibleTreesInForest = MapAllVisibleTrees(forest);
                var sum = SumAllVisibleTreesInForest(visibleTreesInForest);

                // Task 1
                Console.WriteLine("TASK 1");
                var watch = System.Diagnostics.Stopwatch.StartNew();
                var result = sum; // WRONG Answer: 1975 - Too high! 1793 - Too low!
                watch.Stop();
                Console.WriteLine($"Task 1: {result}. Elapsed time [ms]: {watch.ElapsedMilliseconds}");

                // Console.WriteLine("");
                // Console.WriteLine("TASK 2");
                // watch = System.Diagnostics.Stopwatch.StartNew();
                //
                // result = 1337; // Answer:
                // watch.Stop();
                // Console.WriteLine($"Task 2: {result}. Elapsed time [ms]: {watch.ElapsedMilliseconds}");
            }
            else
            {
                Console.WriteLine($"File not found: '{textFile}'");
            }
        }

        private static int SumAllVisibleTreesInForest(bool[,] visibleTreesInForest)
        {
            var sum = 0;
            for (var i = 0; i < visibleTreesInForest.GetLength(0); i++)
            {
                for (var j = 0; j < visibleTreesInForest.GetLength(1); j++)
                {
                    if (visibleTreesInForest[i, j])
                    {
                        sum += 1;
                    }
                }
            }

            return sum;
        }

        private static bool[] MapAllVisibleTreesOnLine(int[] treeLine)
        {
            var visibleTreesInLine = new bool[treeLine.GetLength(0)];
            for (var i = 0; i < treeLine.GetLength(0); i++)
            {
                if (IsVisibleInLineFromEitherSide(i, treeLine))
                {
                    visibleTreesInLine[i] = true;
                }
            }

            return visibleTreesInLine;
        }

        private static bool[,] MapAllVisibleTrees(int[,] forest)
        {
            var visibleTreesInForest = new bool[forest.GetLength(0), forest.GetLength(1)];
            for (int i = 0; i < forest.GetLength(0); i++)
            {
                for (int j = 0; j < forest.GetLength(1); j++)
                {
                    if (i == 4 && j == 2)
                    {
                        var svada = 0;
                    }

                    if (IsVisibleFromAnyDirection(i, j, forest))
                    {
                        visibleTreesInForest[i, j] = true;
                    }
                }
            }

            return visibleTreesInForest;
        }

        private static bool IsVisibleFromAnyDirection(int treeIndexRow, int treeIndexCol, int[,] forest)
        {
            if (IsOnEdge(treeIndexRow, treeIndexCol, forest))
            {
                return true;
            }

            var treeRow = new int[forest.GetLength(0)];
            var treeCol = new int[forest.GetLength(1)];
            for (var i = 0; i < forest.GetLength(0); i++)
            {
                treeRow[i] = forest[treeIndexRow, i];
            }

            for (var j = 0; j < forest.GetLength(1); j++)
            {
                treeCol[j] = forest[j, treeIndexCol];
            }

            var visibleOnRow = IsVisibleInLineFromEitherSide(treeIndexRow, treeRow);
            var visibleOnCol = IsVisibleInLineFromEitherSide(treeIndexCol, treeCol);
            return IsVisibleInLineFromEitherSide(treeIndexRow, treeRow) ||
                   IsVisibleInLineFromEitherSide(treeIndexCol, treeCol);
        }

        private static bool IsOnEdge(int treeIndexRow, int treeIndexCol, int[,] forest)
        {
            if (treeIndexRow == 0 || treeIndexRow == forest.GetLength(0) - 1)
            {
                return true;
            }

            return treeIndexCol == 0 || treeIndexCol == forest.GetLength(1) - 1;
        }

        private static bool IsVisibleInLineFromEitherSide(int treeIndex, IReadOnlyList<int> treeLine)
        {
            var fromLeft = IsVisibleInLineFromOneSide(treeIndex, treeLine);
            var fromRight = IsVisibleInLineFromOneSide(treeLine.Count() - treeIndex - 1, treeLine.Reverse().ToArray());
            
            
            return IsVisibleInLineFromOneSide(treeIndex, treeLine) ||
                   IsVisibleInLineFromOneSide(treeLine.Count - treeIndex - 1, treeLine.Reverse().ToList());
        }

        private static bool IsVisibleInLineFromOneSide(int treeIndex, IReadOnlyList<int> treeLine)
        {
            if (treeIndex == 0) return true; //All edge trees are visible
            if (treeIndex < 0) throw new IndexOutOfRangeException("Index can't be less than zero");

            for (var i = 0; i < treeIndex; i++)
            {
                if (treeLine[i] >= treeLine[treeIndex])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
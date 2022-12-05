using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC2022.Day5
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
                var lines = File.ReadLines(textFile).ToList();

                // Task 1
                // Initial manipulation of input text file - there are probably more elegant ways
                var numStackLines = 0;
                var stackCounterIndex = 0;
                var numInstructionLines = 0;
                for (var i = 0; i < lines.Count; i++)
                {
                    if (lines[i].Contains("[")) numStackLines += 1;
                    if (lines[i].Length < 1) stackCounterIndex = i - 1;
                    if (lines[i].Contains("move")) numInstructionLines += 1;
                }

                // Parse stack drawing to array of stacks, let array index also be Stack ID
                var stackLines = lines.GetRange(0, numStackLines); // Pick the stack drawing
                var stacksCount = Regex.Matches(lines[stackCounterIndex], @"[0-9]").Count;
                var stackIds =
                    new Dictionary<int, int>(); // Connect crate index in string with stack ID - there are probably more elegant ways
                var stacks = new Stack<char>[stacksCount];
                for (var i = 0; i < stacksCount; i++)
                {
                    stacks[i] = new Stack<char>(); // Create empty stacks
                    stackIds.Add(1 + 4 * i, i + 1); // Stack/crate index in stack drawing: 1, 5, 9, ... 
                }

                // Populate stacks by pushing
                // Mind the initial stack order! They're populated in the reverse order (top to bottom in figure on bottom. 
                foreach (var stackLine in stackLines.Reverse<string>())
                {
                    MatchCollection matches = Regex.Matches(stackLine, @"[A-Z]");
                    foreach (Match match in matches)
                    {
                        var crateType = match.Value.ToCharArray().First();
                        var stackIndex = match.Index;
                        stacks[stackIds[stackIndex] - 1].Push(crateType);
                    }
                }

                // Extract instructions from initial input list
                var instructionLines = lines.GetRange(stackCounterIndex + 2, numInstructionLines);
                foreach (var instruction in instructionLines)
                {
                    MatchCollection matches = Regex.Matches(instruction, @"\d+");
                    var numberOfMoves = int.Parse(matches[0].Value);
                    var fromStackIndex = int.Parse(matches[1].Value) - 1;
                    var toStackIndex = int.Parse(matches[2].Value) - 1;

                    // Move one crate at a time between stacks
                    for (var i = 0; i < numberOfMoves; i++)
                    {
                        var crateType = stacks[fromStackIndex].Pop();
                        stacks[toStackIndex].Push(crateType);
                    }
                }

                var endMessage = stacks.Aggregate("", (current, stack) => current + stack.Pop());

                Console.WriteLine("TASK 1");
                var watch = System.Diagnostics.Stopwatch.StartNew();
                var result = endMessage; // Answer: VWLCWGSDQ
                watch.Stop();
                Console.WriteLine($"Task 1: {result}. Elapsed time [ms]: {watch.ElapsedMilliseconds}");

                // Console.WriteLine("");
                // Console.WriteLine("TASK 2");
                // watch = System.Diagnostics.Stopwatch.StartNew();
                //
                // var overLapping = lines.Select(item => item.Split(',')).Count(Overlaps);
                //
                // result = overLapping; // Answer:
                // watch.Stop();
                // Console.WriteLine($"Task 2: {result}. Elapsed time [ms]: {watch.ElapsedMilliseconds}");
            }
            else
            {
                Console.WriteLine($"File not found: '{textFile}'");
            }
        }

        // Heavily inspired by https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/iterators#using-iterators-with-a-generic-list
        private class Stack<T> : IEnumerable<T>
        {
            private readonly T[] _values = new T[100];
            private int _top;

            public void Push(T t)
            {
                _values[_top] = t;
                _top++;
            }

            public T Pop()
            {
                _top--;
                return _values[_top];
            }

            // This method implements the GetEnumerator method. It allows
            // an instance of the class to be used in a foreach statement.
            public IEnumerator<T> GetEnumerator()
            {
                for (int index = _top - 1; index >= 0; index--)
                {
                    yield return _values[index];
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            /// <summary>
            /// Get entire stack
            /// </summary>
            public IEnumerable<T> TopToBottom => this;

            /// <summary>
            /// Get entire stack in reversed order
            /// </summary>
            public IEnumerable<T> BottomToTop
            {
                get
                {
                    for (var index = 0; index <= _top - 1; index++)
                    {
                        yield return _values[index];
                    }
                }
            }

            /// <summary>
            /// Get N stack elements counting from top of stack (not popping)
            /// </summary>
            /// <param name="itemsFromTop"></param>
            /// <returns></returns>
            public IEnumerable<T> TopN(int itemsFromTop)
            {
                // Return less than itemsFromTop if necessary.
                var startIndex = itemsFromTop >= _top ? 0 : _top - itemsFromTop;

                for (var index = _top - 1; index >= startIndex; index--)
                {
                    yield return _values[index];
                }
            }
        }
    }
}
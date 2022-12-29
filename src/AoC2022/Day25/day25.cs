using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC2022.Day25
{
    public static class Main
    {
        public static void Solve(int dayNumber)
        {
            Console.WriteLine($"Day {dayNumber}:");

            var filename = "day" + Convert.ToString(dayNumber);

            var textFile = $"../../../../../resources/{filename}.txt";
            //var textFile = $"../../../../../resources/{filename}_example.txt";

            if (File.Exists(textFile))
            {
                // Task 1
                Console.WriteLine("TASK 1");
                var watch = System.Diagnostics.Stopwatch.StartNew();

                var result =
                    File.ReadLines(textFile).Select(ConvertFromSNAFUToDecimal).Sum()
                        .ToSNAFU(); // Answer: 2----0=--1122=0=0021

                watch.Stop();
                Console.WriteLine($"Task 1: {result}. Elapsed time [ms]: {watch.ElapsedMilliseconds}");
            }
            else
            {
                Console.WriteLine($"File not found: '{textFile}'");
            }
        }

        public static long ConvertFromSNAFUToDecimal(string snafuVal)
        {
            var snafuSymbols2DecimalDigits = new Dictionary<char, long>
                {{'=', -2}, {'-', -1}, {'0', 0}, {'1', 1}, {'2', 2}};

            return snafuVal
                .Select((t, i) => snafuSymbols2DecimalDigits[t] * (long) Math.Pow(5, snafuVal.Length - 1 - i)).Sum();
        }

        public static string ToSNAFU(this long decVal)
        {
            return decVal.ToBase(5).ToLongArray().ToSNAFUDigits().ToSNAFUSymbols();
        }

        public static string ToBase(this long decVal, long radix)
        {
            var backwards = "";
            while (decVal >= 1)
            {
                var val = decVal / radix;
                // For the operands of integer types, the result of a % b is the value produced by a - (a / b) * b
                var rem = decVal % radix;
                backwards += rem.ToString();
                decVal = val;
            }

            var otherBaseVal = "";
            for (var i = backwards.Length - 1; i >= 0; i--)
            {
                otherBaseVal += backwards[i];
            }

            return otherBaseVal;
        }

        public static long[] ToLongArray(this string sNumber)
        {
            return sNumber.Select(n => (long) (n - '0')).ToArray();
        }

        public static long[] ToSNAFUDigits(this long[] decNumber)
        {
            // Array needs to be long enough to handle potential spill-over to the Most Significant Digit
            var longArray = new long[decNumber.Length + 1];
            Array.Copy(decNumber, 0, longArray, 1, decNumber.Length);

            var decimalDigits2SnafuDigits = new Dictionary<long, string>
                {{-2, "="}, {-1, "-"}, {0, "0"}, {1, "1"}, {2, "2"}};
            // 5 is not really a troublesome remainder, but it is easier to add here than to perform a sub-conversion to "10".
            var troublesomeRemainders = new Dictionary<long, string> {{3, "1="}, {4, "1-"}, {5, "10"}};

            for (var i = longArray.Length - 1; i >= 0; i--)
            {
                // Check if decimal multiplier has an equivalent SNAFU multiplier
                if (decimalDigits2SnafuDigits.ContainsKey(longArray[i])) continue;

                // Otherwise, operate on troublesome remainders: 3 --> 1= --> +1, -2, 4 --> 1- --> +1, -1
                if (longArray[i] == 3) longArray[i] = -2;
                if (longArray[i] == 4) longArray[i] = -1;
                if (longArray[i] == 5) longArray[i] = 0;
                longArray[i - 1] += 1;

                if (!decimalDigits2SnafuDigits.ContainsKey(longArray[i]) &&
                    !troublesomeRemainders.ContainsKey(longArray[i]))
                {
                    throw new ArgumentOutOfRangeException($"longArray[{i}]", longArray[i],
                        "Not an accepted SNAFU digit.");
                }
            }

            if (longArray[0] != 0) return longArray;
            Array.Copy(longArray, 1, decNumber, 0, decNumber.Length);
            return decNumber;
        }

        public static string ToSNAFUSymbols(this IEnumerable<long> iNumber)
        {
            var decimalDigits2SnafuDigits = new Dictionary<long, string>
                {{-2, "="}, {-1, "-"}, {0, "0"}, {1, "1"}, {2, "2"}};

            return iNumber.Aggregate("", (current, t) => current + decimalDigits2SnafuDigits[t]);
        }
    }
}
using Xunit;
using AoC2022.Day25;

namespace TestAoC
{
    public class TestDecimalToSNAFU
    {
        private const int SnafuRadix = 5;

        [Fact]
        public void DecimalToSnafuWithNormalDigits()
        {
            long[] testDecimals = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 15, 20, 2022, 12345, 314159265};
            string[] snafuResults =
            {
                "1", "2", "1=", "1-", "10", "11", "12", "2=", "2-", "20", "1=0", "1-0", "1=11-2", "1-0---0",
                "1121-1110-1=0"
            };
            for (var i = 0; i < testDecimals.Length; i++)
            {
                Assert.Equal(snafuResults[i], testDecimals[i].ToSNAFU());
            }
        }

        [Fact]
        public void DecimalToSnafuWithDecimalDigits()
        {
            // Part of process of converting Decimal 4890 to SNAFU 2=-1=0
            Assert.Equal("124030", Main.ToBase(4890, SnafuRadix));
        }

        [Fact]
        public void TestStringNumberToLongArray()
        {
            // Part of process of converting Decimal 4890 to SNAFU 2=-1=0
            Assert.Equal(new long[] {1, 2, 4, 0, 3, 0}, "124030".ToLongArray());
        }

        [Fact]
        public void TestSnafuDecimalDigitsToSnafuDigits()
        {
            // Part of process of converting Decimal 4890 to SNAFU 2=-1=0
            Assert.Equal(new long[] {2, -2, -1, 1, -2, 0},
                new long[] {1, 2, 4, 0, 3, 0}.ToSNAFUDigits());
        }

        [Fact]
        public void TestSnafuDigitsToSnafuSymbols()
        {
            // Part of process of converting Decimal 4890 to SNAFU 2=-1=0
            Assert.Equal("2=-1=0", new long[] {2, -2, -1, 1, -2, 0}.ToSNAFUSymbols());
        }
    }
}
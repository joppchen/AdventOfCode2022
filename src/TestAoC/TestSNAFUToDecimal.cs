using AoC2022.Day25;
using Xunit;

namespace TestAoC
{
    public class TestSNAFUToDecimal
    {
        [Fact]
        public void SNAFUToDecimal()
        {
            Assert.Equal(3, Main.ConvertFromSNAFUToDecimal("1="));
            Assert.Equal(1747, Main.ConvertFromSNAFUToDecimal("1=-0-2"));

            int[] decimals = {1747, 906, 198, 11, 201, 31, 1257, 32, 353, 107, 7, 3, 37};
            string[] snafus =
                {"1=-0-2", "12111", "2=0=", "21", "2=01", "111", "20012", "112", "1=-1=", "1-12", "12", "1=", "122"};
            for (var i = 0; i < decimals.Length; i++)
            {
                Assert.Equal(decimals[i], Main.ConvertFromSNAFUToDecimal(snafus[i]));
            }
        }
    }
}
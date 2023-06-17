using System;
using System.Text;

namespace Util
{
    public static class Util
    {
        public static Random Rand {get;} = new Random();

        public static string GetSamples(int[] arrayToPrint)
        {
            StringBuilder sampleString = new StringBuilder("[");
            foreach (var value in arrayToPrint)
            {
                sampleString.Append($" {value}");
            }
            sampleString.Append(" ]");

            return sampleString.ToString();
        }
    }
}

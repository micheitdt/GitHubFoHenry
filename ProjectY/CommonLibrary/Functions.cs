using System;

namespace CommonLibrary
{
    public class Functions
    {
        public static int BitsToInt(bool b1, bool b2)
        {
            return 2 * (b1 == true ? 1 : 0) + (b2 == true ? 1 : 0);
        }

        public static int BitsToInt(bool b1, bool b2, bool b3)
        {
            return 4 * (b1 == true ? 1 : 0) + 2 * (b2 == true ? 1 : 0) + (b3 == true ? 1 : 0);
        }

        public static bool GetBit(byte b, int bitNumber)
        {
            return (b & (1 << bitNumber)) != 0;
        }

        private static int BCDToInt(byte bcd)
        {
            return (0xff & (bcd >> 4)) * 10 + (0xf & bcd);
        }

        public static int ConvertToFormat9(byte[] stream, int startIndex, int length)
        {
            int output = 0;
            for (int i = 0; i < length; i++)
            {
                output += BCDToInt(stream[startIndex + i]) * Convert.ToInt32(Math.Pow(100, length - 1 - i));
            }
            return output;
        }

        public static long ConvertToFormat9L(byte[] stream, int startIndex, int length)
        {
            long output = 0;
            for (int i = 0; i < length; i++)
            {
                output += BCDToInt(stream[startIndex + i]) * Convert.ToInt64(Math.Pow(100, length - 1 - i));
            }
            return output;
        }
    }
}

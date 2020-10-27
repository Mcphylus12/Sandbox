using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Scorer
{
    class MainC
    {
        public static void Main(string[] args)
        {
            Console.Write(DigitalRoot(16));
        }

        public static int DigitalRoot(long n)
        {
            long num = n;

            while (num / 10 > 0)
            {
                long sum = 0;
                long mod = num % 10;
                while (num > 0)
                {
                    sum += mod;
                    num -= mod;
                    num /= 10;
                }

                num = sum;
            }

            return (int)num;
        }
    }
}

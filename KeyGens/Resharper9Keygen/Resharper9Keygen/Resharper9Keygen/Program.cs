using System;
using System.Diagnostics;
using System.Linq;
using Mono.Math;

namespace Resharper9Keygen
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var username = "xor";
            var version = (decimal)9.1;

            Debug.WriteLine(GenerateLicenseKey(username, version));
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        private static string GenerateLicenseKey(string username, decimal version)
        {
            username = username.Trim();
            if (string.IsNullOrEmpty(username))
            {
                return string.Empty;
            }

            var value = (int)version;
            var num = (int)((version - value) * new decimal(10));
            var num1 = username.Aggregate(0, (current, t) => ((current << 7) + t)%0xfff1);

            var bigInteger = new BigInteger(Uid(username));
            bigInteger.SetBit(0);

            var bigInteger1 = new BigInteger();
            bigInteger1 = bigInteger1 + (new BigInteger(1) << 88);
            bigInteger1 = bigInteger1 + (new BigInteger(3) << 120);
            bigInteger1 = bigInteger1 + (new BigInteger(0xffff) << 56);
            bigInteger1 = bigInteger1 + (new BigInteger(0xfe7f) << 136);
            bigInteger1 = bigInteger1 + (new BigInteger(0) << 40);
            bigInteger1 = bigInteger1 + (new BigInteger(value * 0x3e8 + num) << 72);
            bigInteger1 = bigInteger1 + (new BigInteger(0) << 32);
            bigInteger1 = bigInteger1 + (new BigInteger(0) << 16);
            bigInteger1 = bigInteger1 + new BigInteger(num1);

            var bigInteger2 = BigInteger.Parse("3483968730802868401158985191529641621586542542912639916793");
            var bigInteger3 = (BigInteger.Parse("48625079161695385866568653587") - 1) * (BigInteger.Parse("71649625889912785451603068739") - 1);
            bigInteger1 = bigInteger1.ModPow(bigInteger.ModInverse(bigInteger3), bigInteger2);
            return string.Format("{0}-{1}", 1, Convert.ToBase64String(bigInteger1.GetBytes()));
        }

        private static byte[] Uid(string s)
        {
            var numArray = new byte[s.Length];
            for (var i = 0; i < s.Length; i++)
            {
                numArray[i] = (byte)(s[i] & '\u007F');
            }
            return numArray;
        }
    }
}

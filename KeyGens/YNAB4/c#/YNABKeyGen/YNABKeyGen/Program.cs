using System;

namespace YNABKeyGen
{
    class Program
    {
        static void Main()
        {
            var keygen = new YNAB4.KeyGen();

            for (var i = 0; i < 10; i++)
            {
                Console.WriteLine(keygen.GenerateActivationKey());
            }

            Console.ReadKey();
        }
    }

    namespace YNAB4
    {
        using Org.BouncyCastle.Crypto.Engines;
        using Org.BouncyCastle.Crypto.Modes;
        using Org.BouncyCastle.Crypto;
        using Org.BouncyCastle.Crypto.Parameters;
        using System.Text;

        public class KeyGen
        {
            private const string EncKey = "CHANGEDFORYNAB4WEKNOWTHISISN'TSECUREBUTIFTHEYAREUSINGAKEYGENTHEYWOULDNTBUYITANYWAY";
            private readonly byte[] IV = { 0x91, 0x17, 0xCB, 0x3E, 0xD9, 0x7F, 0x57, 0x76 };

            private static readonly Random Prng = new Random();

            public string GenerateActivationKey()
            {
                var engine = new BlowfishEngine();
                var cipher = new CbcBlockCipher(engine);
                var bbc = new BufferedBlockCipher(cipher);
                bbc.Init(true, new ParametersWithIV(new KeyParameter(Encoding.ASCII.GetBytes(EncKey)), IV));

                var n = Prng.Next(0, 999999);

                var s = String.Format("{0,6};YNAB4;;;;", n); // must be fixed length due to padding issue
                var sb = Encoding.ASCII.GetBytes(s);
                sb[sb.Length - 4] = 0x4; // 
                sb[sb.Length - 3] = 0x4; // padding issue???
                sb[sb.Length - 2] = 0x4; // PCKS#5
                sb[sb.Length - 1] = 0x4; //
                var cipherText = new byte[bbc.GetOutputSize(sb.Length)];
                var outputLen = bbc.ProcessBytes(sb, 0, sb.Length, cipherText, 0);
                bbc.DoFinal(sb, outputLen);
                var encryptedLic = Base32.EncodeByteArray(cipherText);

                return encryptedLic;
            }
        }
    }

}

using System;

namespace YNABKeyGen
{
    public static class Base32
    {
        private const string ValidChars = "QAZ2WSX3EDC4RFV5TGB6YHN7UJM8K9LP";

        public static String EncodeByteArray(byte[] bytes)
        {
            var sb = "";
            var hi = 5;
            var currentByte = 0;
            while (currentByte < bytes.Length)
            {
                int index;
                if (hi > 8)
                {
                    index = bytes[currentByte++] >> hi - 5;
                    if (currentByte != bytes.Length)
                    {
                        index = bytes[currentByte] << 16 - hi >> 3 | index;
                    }
                    hi = hi - 3;
                }
                else if (hi == 8)
                {
                    index = bytes[currentByte++] >> 3;
                    hi = hi - 3;
                }
                else
                {
                    index = bytes[currentByte] << 8 - hi >> 3;
                    hi = hi + 5;
                }

                sb = sb + ValidChars[index];
            }
            return sb;
        }
    }
}
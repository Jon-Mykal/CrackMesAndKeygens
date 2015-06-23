using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace LogiclyKeygen
{
    class Program
    {
        private const string AllowedProductKeyCharacters = "BCDFGHJKMPQRTVWXY2346789";

        private static void Main(string[] args)
        {
            char[] charset = AllowedProductKeyCharacters.ToCharArray();
            const int keyLen = 25;
            double keyspace = Math.Pow(keyLen, charset.Length);

            int[] curPas = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            for (var i = 0; i < keyspace; i++)
            {
                var p = BuildPass(curPas);
                
                if (CheckKey(p, 0, 23, 0, 1))
                {
                    Debug.WriteLine(p);
                }

                curPas = IncrementPosition(curPas, 0);
            }
        }


        private static string BuildPass(int[] positions)
        {
            var pass = new char[positions.Length];

            for (var i = 0; i < pass.Length; i++)
            {
                pass[i] = AllowedProductKeyCharacters[positions[i]];
            }

            var rep = "0123456789abcdef";
            var temp = new string(pass);
            var sha256 = Sha256(temp);
            var subsha = sha256.Substring(51);

            pass = new char[positions.Length*2+1];

            int cindex = 0;
            for (var i = 0; i < pass.Length; i++)
            {
                if (i%2 ==0)
                {
                    pass[i] = AllowedProductKeyCharacters[rep.IndexOf(subsha[cindex])];
                }
                else
                {
                    pass[i] = temp[cindex];
                    cindex ++;
                }
            }

            return new string(pass);
        }

        private static int[] IncrementPosition(int[] positions, int x)
        {
            if (x >= positions.Length || x < 0)
            {
                throw new ArgumentOutOfRangeException("x");
            }

            positions[x]++;

            if (x == 0 || x == 1)
            {
                if (positions[x] >= 12)
                {
                    positions[x] = 0;
                    positions = IncrementPosition(positions, x + 1);
                }
            }
            else if (x%2 == 0)
            {
                if (positions[x] >= 16)
                {
                    positions[x] = 0;
                    positions = IncrementPosition(positions, x + 1);
                }
            }
            else
            {
                if (positions[x] >= AllowedProductKeyCharacters.Length)
                {
                    positions[x] = 0;
                    positions = IncrementPosition(positions, x + 1);
                }
            }

            return positions;
        }

        private static string Sha256(string text)
        {
            using (SHA256 hash = SHA256Managed.Create())
            {
                return String.Join("", hash
                  .ComputeHash(Encoding.UTF8.GetBytes(text))
                  .Select(item => item.ToString("x2")));
            }
        }

        // Recreated from Logicly code.
        private static bool CheckKey(string p1, int p2 = 0, int p3 = 2147483647, int p4 = 0, int p5 = 2147483647)
        {
            if (string.IsNullOrEmpty(p1))
            {
                return false;
            }

            p1 = p1.Replace("-", "");
            int _loc6_ = 25;
            if (p1.Length != _loc6_)
            {
                return false;
            }

            int _loc7_ = 0;
            while (_loc7_ < _loc6_)
            {
                if (AllowedProductKeyCharacters.IndexOf(p1[_loc7_]) < 0)
                {
                    return false;
                }
                _loc7_++;
            }

            string _loc8_ = "0123456789abcdef"; // 466fbcd572053
            string _loc9_ = "";
            string _loc10_ = "";

            _loc7_ = 0;
            while (_loc7_ < _loc6_)
            {
                if (_loc7_ % 2 == 0)
                {
                    char _loc18_ = p1[_loc7_];
                    int _loc19_ = AllowedProductKeyCharacters.IndexOf(_loc18_);
                    _loc9_ += _loc8_[_loc19_];
                }
                else
                {
                    _loc10_ += p1[_loc7_];
                }
                _loc7_++;
            }

            if (string.IsNullOrEmpty(_loc9_))
            {
                return false;
            }

            string _loc11_ = Sha256(_loc10_);
            if (_loc11_.LastIndexOf(_loc9_) != _loc11_.Length - _loc9_.Length)
            {
                return false;
            }

            int _loc12_ = AllowedProductKeyCharacters.IndexOf(_loc10_[0]);
            int _loc13_ = AllowedProductKeyCharacters.IndexOf(_loc10_[1]);
            string _loc14_ = _loc10_[_loc12_].ToString();
            string _loc15_ = _loc10_[_loc13_].ToString();
            int _loc16_ = AllowedProductKeyCharacters.IndexOf(_loc14_);
            int _loc17_ = -(AllowedProductKeyCharacters.IndexOf(_loc15_) + 1 - AllowedProductKeyCharacters.Length);

            if (_loc16_ < p2 || _loc16_ > p5)
            {
                return false;
            }

            if (_loc17_ < p4 || _loc17_ > p5)
            {
                return false;
            }

            return true;
        }
    }
}

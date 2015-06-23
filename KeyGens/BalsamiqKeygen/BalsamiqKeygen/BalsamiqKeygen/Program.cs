using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using Ionic.Zlib;

namespace BalsamiqKeygen
{
    /// <summary>
    /// Uses CGenT.Zlib library for decompression of key string.
    /// https://www.nuget.org/packages/CGenT.ZLib/1.10.5548.39415
    /// </summary>
    class Program
    {
        /*  Mark license as commercial (and 'kind' other than 'T'), to get permanent activation.
         * 
         *  Following key pairs are black listed:
         * 
         *    this._blacklist["foosoft"] = "eNrzzU/OLi0odswsqknLzy/OTyupMTQyNjYxNDczAIEa5xpDAPTaC+4=";
         *    this._blacklist["haitao14@gmail.com"] = "eNrzzU/OLi0odswsqslIzCxJzDc0cUjPTczM0UvOz60xNDI2MbY0NjIAgRrnGkMAmR0PgA==";
         *    this._blacklist["hxds143@yahoo.com.cn"] = "eNrzzU/OLi0odswsqsmoSCk2NDF2qEzMyM/XS87P1UvOqzE0MjU1MzczMQCBGucaQwC38xAS";
         *    this._blacklist["leexij@gmail.com"] = "eNrzzU/OLi0odswsqslJTa3IzHJIz03MzNFLzs+tMTQyNrcwsTQyAIEa5xpDAIFxDy8=";
         *    this._blacklist["www.serials.ws"] = "eJzzzU/OLi0odswsqikvL9crTi3KTMwp1isvrrFEAjXONYY1fu6ufgCmMhBR";
         *    this._blacklist["Canonical UX"] = "eNrzzU/OLi0odswsqnFOzMvPy0xOzFEIjagxNDIxMQASBiBQ41xjaAgAQYUNaw==";
         *    this._blacklist["leexij@gmail.com"] = "eNrzzU/OLi0odswsqslJTa3IzHJIz03MzNFLzs+tMTQyNrcwsTQyAIEa5xpDAIFxDy8=";
         *    this._blacklist["Rick Dong"] = "eNrzzU/OLi0odswsqgnKTM5WcMnPS1eoMTQyMjexMDQyAIEa5xpDAA8pDD8=";
        */

        static void Main(string[] args)
        {
            var keyData1 = new KeyData("MockupsAir", "Evilzone", DateTime.Now, "C", "1");
            Debug.WriteLine(keyData1);
            Debug.WriteLine(keyData1.Encode());
            Debug.WriteLine("");

            var keyData2 = KeyData.Decode(keyData1.Encode());
            Debug.WriteLine(keyData2);
            Debug.WriteLine(keyData2.Encode());

            Console.ReadKey();
        }
    }

    public class KeyData
    {
        public string productName;
        public string companyName;
        public DateTime startDate;
        public string kind;
        public string users;

        public KeyData(string productName, string companyName, DateTime startDate, string kind, string users)
        {
            this.productName = productName;
            this.companyName = companyName;
            this.startDate = startDate;
            this.kind = kind;
            this.users = users;
        }

        public bool commercial
        {
            get { return kind != null && kind[0] != 'T'; }
        }

        public override string ToString()
        {
            var dks = String.Format("{0}|{1}|{2}|{3}|{4}", productName, companyName, startDate.Ticks, kind, users);
            return dks;
        }

        public string Encode()
        {
            var dks = String.Format("{0}|{1}|{2}|{3}|{4}", productName, companyName, startDate.Ticks, kind, users);
            var cks = ZlibStream.CompressString(dks);
            return Convert.ToBase64String(cks);
        }

        public static KeyData Decode(string base64)
        {
            var bsd = Convert.FromBase64String(base64);
            var uks = ZlibStream.UncompressBuffer(bsd);
            var dks = Encoding.UTF8.GetString(uks);
            var parts = dks.Split(new[] {"|"}, StringSplitOptions.None);

            if (parts.Length < 5)
            {
                return null;
            }

            long ticks;
            if (!long.TryParse(parts[2], out ticks))
            {
                return null;
            }

            return new KeyData(parts[0], parts[1], new DateTime(ticks), parts[3], parts[4]);
        }
    }

}

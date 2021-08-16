using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DataServer.Common.Utilities
{
    public class Sha1Utilities
    {
        public static string CalculateHash(string text)
        {
            SHA1Managed sha1 = new SHA1Managed();
            var textBytes = Encoding.UTF8.GetBytes(text);
            var hashAsBytes = sha1.ComputeHash(textBytes);
            var stringBuilder = new StringBuilder(hashAsBytes.Length * 2);
            foreach (byte b in hashAsBytes)
            {
                // can be "x2" if you want lowercase
                stringBuilder.Append(b.ToString("x2"));
            }
            return stringBuilder.ToString();
        }
    }
}

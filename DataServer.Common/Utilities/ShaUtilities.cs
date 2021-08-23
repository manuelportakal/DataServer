using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DataServer.Common.Utilities
{
    public class ShaUtilities
    {
        public static string CalculateSha1Hash(string text)
        {
            SHA1Managed sha1 = new SHA1Managed();
            var textBytes = Encoding.UTF8.GetBytes(text);
            var hashAsBytes = sha1.ComputeHash(textBytes);
            return GetReadableHash(hashAsBytes);
        }

        public static string CalculateSha256Hash(string text)
        {
            SHA256 mySHA256 = SHA256.Create();
            var textBytes = Encoding.UTF8.GetBytes(text);
            byte[] hashAsBytes = mySHA256.ComputeHash(textBytes);

            return GetReadableHash(hashAsBytes);
        }

        public static string CalculateHmac256(string key, string message)
        {
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var messageBytes = Encoding.UTF8.GetBytes(message);
            HMACSHA256 hmac = new HMACSHA256(keyBytes);
            byte[] hashAsBytes = hmac.ComputeHash(messageBytes);

            return GetReadableHash(hashAsBytes);
        }

        private static string GetReadableHash(byte[] hashAsBytes)
        {
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

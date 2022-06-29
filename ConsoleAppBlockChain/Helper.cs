using System;
using System.Runtime.Serialization;
using System.Text;
using XSystem.Security.Cryptography;

namespace ConsoleAppBlockChain
{
    [DataContract]
    public class Helper
    {
        protected string GetStringForHash(params string[] data)
        {
            string strHash = "";

            for (int i = 0; i < data.Length; i++)
                strHash += data[i];

            return strHash;
        }

        protected string GetHash(string strHash)
        {
            byte[] messageToByte = Encoding.ASCII.GetBytes(strHash);
            SHA256Managed sha256Obj = new SHA256Managed();

            var hashValue = sha256Obj.ComputeHash(messageToByte);

            string hex = "";
            foreach (var value in hashValue)
                hex += String.Format("{0:x2}", value);

            return hex;
        }
    }
}
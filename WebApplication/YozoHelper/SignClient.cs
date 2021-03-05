using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace WebApplication
{
    /// <summary>
    /// 生成验签数据 sign
    /// </summary>
    public class Signclient
    {
        public static string generateSign(string secret, Dictionary<string, string[]> paramMap)
        {
            string fullParamStr = uniqSortParams(paramMap);
            return HmacSHA256(fullParamStr, secret);
        }
        public static string uniqSortParams(Dictionary<string, string[]> paramMap)
        {
            paramMap.Remove("sign");
            paramMap = paramMap.OrderBy(o => o.Key).ToDictionary(o => o.Key.ToString(), p => p.Value);
            StringBuilder strB = new StringBuilder();
            foreach (KeyValuePair<string, string[]> kvp in paramMap)
            {
                string key = kvp.Key;
                string[] value = kvp.Value;
                if (value.Length > 0)
                {
                    Array.Sort(value);
                    foreach (string temp in value)
                    {
                        strB.Append(key).Append("=").Append(temp);
                    }
                }
                else
                {
                    strB.Append(key).Append("=");
                }

            }
            return strB.ToString();
        }
        public static string HmacSHA256(string data, string key)
        {
            string signRet = string.Empty;
            using (HMACSHA256 mac = new HMACSHA256(Encoding.UTF8.GetBytes(key)))
            {
                byte[] hash = mac.ComputeHash(Encoding.UTF8.GetBytes(data));
                signRet = ToHexString(hash); ;
            }
            return signRet;
        }
        public static string ToHexString(byte[] bytes)
        {
            string hexString = string.Empty;
            if (bytes != null)
            {
                StringBuilder strB = new StringBuilder();
                foreach (byte b in bytes)
                {
                    strB.AppendFormat("{0:X2}", b);
                }
                hexString = strB.ToString();
            }
            return hexString;
        }
    }

}
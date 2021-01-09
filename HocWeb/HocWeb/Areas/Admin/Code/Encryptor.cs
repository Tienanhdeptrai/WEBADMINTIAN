using System;
using System.Security.Cryptography;
using System.Text;
namespace HocWeb.Areas.Admin.Code
{
    public static class Encryptor
    {
        public static string MD5Hash(string text)
        {
            try
            {
                MD5 md5 = new MD5CryptoServiceProvider();
                md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));
                byte[] result = md5.Hash;
                StringBuilder strbuilder = new StringBuilder();
                for (int i = 0; i < result.Length; i++)
                {
                    strbuilder.Append(result[i].ToString("x2"));

                }
                return strbuilder.ToString();
            }
            catch (Exception)
            {
                string a = null;
                return a;
            }
           
        }
       
    }
}
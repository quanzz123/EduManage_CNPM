using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace eduManage.Utilities
{
    public class Functions
    {
        public static int _UserId = 0;
        public static string _UserName = String.Empty;
        public static string _Email = String.Empty;
        public static string _Message = string.Empty;
        public static string _MessageEmail = string.Empty;
        public static string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            md5.ComputeHash(System.Text.Encoding.ASCII.GetBytes(text));
            byte[] result = md5.Hash;
            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                strBuilder.Append(result[i].ToString("x2"));

            }
            return strBuilder.ToString();
        }

        public static string MD5Password(string? text)
        {
            string str = MD5Hash(text)
            for (int i = 0; i < 1000; i++)
                {
                    str = MD5Hash(str+ "_" + str);
                }
            return str;
        }
    }
}

using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Security.Cryptography;
using System.Text;

namespace MediLink.Resources
{
    public class Utilities
    {
        //convert password to sha256
        public static string EncryptPassword(string text)
        {
            string hash = string.Empty;

            using (SHA256 sha256 = SHA256.Create())
            {
                //get text hash 
                byte[] hashvalue = sha256.ComputeHash(Encoding.UTF8.GetBytes(text));

                //convert array byte to text
                foreach (byte b in hashvalue)
                {
                    hash += $"{b:X2}";

                }
            }
            return hash;
        }

        //generate token 
        public static string GenerateToken()
        {
            string token = Guid.NewGuid().ToString("N");
            return token;
        }
    }
}

using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using System.Text;

namespace WebFood.Utility
{
    public static class PasswordHasher
    {
        public static string Hash(string password)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] hashValue = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToHexString(hashValue);
            }
        }
    }
}

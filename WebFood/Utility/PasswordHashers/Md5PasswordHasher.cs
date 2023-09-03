using System.Security.Cryptography;
using System.Text;

namespace WebFood.Utility.PasswordHashers
{
    public class Md5PasswordHasher : IPasswordHasherStrategy
    {
        public string Hash(string password)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] hashValue = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToHexString(hashValue);
            }
        }
    }
}

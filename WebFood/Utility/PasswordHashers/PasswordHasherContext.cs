using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using System.Text;

namespace WebFood.Utility.PasswordHashers
{
    public class PasswordHasherContext
    {
        private IPasswordHasherStrategy _strategy;

        public PasswordHasherContext(IPasswordHasherStrategy strategy)
        {
            _strategy = strategy;        
        }

        public void SetStrategy(IPasswordHasherStrategy strategy)
        {
           _strategy = strategy;
        }

        public string Hash(string password)
        {
            return _strategy.Hash(password);
        }
    }
}

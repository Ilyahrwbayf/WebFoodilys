namespace WebFood.Utility.PasswordHashers
{
    public interface IPasswordHasherStrategy
    {
        public string Hash(string password);
    }
}

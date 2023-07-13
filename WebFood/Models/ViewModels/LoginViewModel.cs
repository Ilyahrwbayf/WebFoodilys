namespace WebFood.Models.ViewModels
{
    public class LoginViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool KeepLoggedIn { get; set; }

        public LoginViewModel()
        {
            Email = string.Empty;
            Password = string.Empty;
            KeepLoggedIn = false;
        }
    }
}

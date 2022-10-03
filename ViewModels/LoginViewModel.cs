using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace BancoParaleloAPI.ViewModels
{
    public class LoginViewModel
    {
        public string Email { get; set; }
        public string Senha { get; set; }
    }
}

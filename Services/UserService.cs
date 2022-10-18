using BancoParaleloAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace BancoParaleloAPI.Services
{
    public interface IUserService
    {
        Task<Resposta> CriarNovoUsuarioAsync(IdentityUser identityUser, string password);
        Task<IdentityUser> RecuperarUsuarioPorEmail(string email);
    }

    public class UserService : IUserService
    {
        private UserManager<IdentityUser> _userManager;
        private IConfiguration _configuration;
        public UserService(UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration; 
        }

        public async Task<Resposta> CriarNovoUsuarioAsync(IdentityUser identityUser, string password)
        {
            try
            {
                var result = await _userManager.CreateAsync(identityUser, password);
                if (result.Succeeded)
                {
                    return new Resposta
                    {
                        Mensagem = "Usuario Criado com Sucesso!",
                        Sucesso = true
                    };
                }

                return new Resposta
                {
                    Mensagem = "Falha ao criar um usuario",
                    Sucesso = false
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IdentityUser> RecuperarUsuarioPorEmail(string email)
        {
            try
            {
                var usuario = await _userManager.FindByEmailAsync(email);
                return usuario;
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}

using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;
using Org.BouncyCastle.Crypto.Macs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using BancoParaleloAPI.Models;
using BancoParaleloAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Drawing;

namespace BancoParaleloAPI.Services
{
    public interface IEmailService
    {
        Task EnviarEmailAsync(string subject, string toEmail, string message);
        Task EnviarConfirmacaoEmailAsync(IdentityUser identityUser, string signalrid);
        Task<Resposta> ConfirmarEmailAsync(string userId, string token);
        Task<Resposta> EnviarRecuperacaoSenhaAsync(string email, string id);
    }

    public class EmailService : IEmailService
    {
        private readonly ILogger _logger;
        private UserManager<IdentityUser> _userManager;
        private IConfiguration _configuration;

        public EmailService(ILogger<EmailService> logger, IConfiguration configuration, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _configuration = configuration; 
            _userManager = userManager;
        }

        public async Task EnviarEmailAsync(string subject, string toEmail, string message)
        {
            try
            {
                //create email
                var email = new MimeMessage();
                email.From.Add(new MailboxAddress("Banco Paralelo", "bancoparalelo@gmail.com"));
                email.To.Add(MailboxAddress.Parse(toEmail));
                email.Subject = subject;
                email.Body = new TextPart(TextFormat.Html) { Text = message };

                // send email
                using var smtp = new SmtpClient();
                smtp.Connect("smtp.gmail.com", 465, SecureSocketOptions.Auto);
                smtp.Authenticate("bancoparalelo@gmail.com", "wuoeoiobdknclyym");
                await smtp.SendAsync(email);
                smtp.Disconnect(true);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task EnviarConfirmacaoEmailAsync(IdentityUser identityUser, string signalrid)
        {
            try
            {
                var TokenConfirmarEmail = await _userManager.GenerateEmailConfirmationTokenAsync(identityUser);

                var TokenCodificado = Encoding.UTF8.GetBytes(TokenConfirmarEmail);
                var ValidarTokenEmail = WebEncoders.Base64UrlEncode(TokenCodificado);

                string url = $"{_configuration["ClientUrl"]}/emailconfirmado?email={identityUser.Email}&userid={identityUser.Id}&token={ValidarTokenEmail}&signalrid={signalrid}";

                string bodyEmail = @$"<img src=""https://i.ibb.co/vLn3jW2/Banner-Email.png"" alt=""Banner-Email"" border=""0"">
                                      <br>
                                      <h3 style=""color:#673ab7;""> Confirmacao de Email </h3>
                                      <p>Para confirmar o seu email e completar seu cadastro, clique no botao abaixo!</p>
                                      <table bgcolor="" width = ""100 % "" cellspacing = ""0"" cellpadding = ""0"" border = ""0"" >
                                        <tbody>
                                            <tr>
                                                <td height = ""10"" style = ""line - height: 1px; height: 10px; font - size: 1px; "" > &nbsp;</ td >
                                            </tr>
                                            <tr>
                                                <td> 
                                                    <table border = ""0"" cellspacing=""0"" cellpadding=""0""> 
                                                        <tbody> 
                                                            <tr> 
                                                                <td align = ""center"" height=""38"" style=""border-radius: 2px; font-family: 'Segoe UI', SUWR, Arial, Sans-Serif; font-size: 16px; font-weight: 600; line-height: 18px; height: 38px; padding-left: 20px; padding-right: 20px;"" bgcolor=""#673ab7"">
                                                                    <a href=""{url}"" target=""_blank"" style=""color: #ffffff; text-decoration: none;"">
                                                                        <strong style=""color: #ffffff; font-weight: 600; text-decoration: none;"" dir=""ltr"">Clique aqui para confirmar seu email !&nbsp;</strong>
                                                                    </a>
                                                                </td> 
                                                            </tr> 
                                                        </tbody> 
                                                    </table> 
                                                </td> 
                                            </tr> 
                                            <tr> 
                                                <td height = ""10"" style=""line-height: 1px; height: 10px; font-size: 1px;"">&nbsp;</td> 
                                            </tr> 
                                        </tbody> 
                                      </table>";

                await EnviarEmailAsync("Confirmar seu email!", identityUser.Email, bodyEmail);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Resposta> ConfirmarEmailAsync(string userId, string token)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                var TokenDecodificado = WebEncoders.Base64UrlDecode(token);
                string normalToken = Encoding.UTF8.GetString(TokenDecodificado);

                var retorno = await _userManager.ConfirmEmailAsync(user, normalToken);
                if (retorno.Succeeded)
                {
                    return new Resposta
                    {
                        Mensagem = "Email confirmado com sucesso !",
                        Sucesso = true
                    };
                }

                return new Resposta
                {
                    Mensagem = "Falha ao confirmar o email!",
                    Sucesso = false
                };

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Resposta> EnviarRecuperacaoSenhaAsync(string email, string Id)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    return new Resposta
                    {
                        Mensagem = "Usuario Nao Existe!",
                        Sucesso = false
                    };
                }

                //token == id usuario
                var token = Id;
                var TokenCodificado = Encoding.UTF8.GetBytes(token);
                var TokenValido = WebEncoders.Base64UrlEncode(TokenCodificado);

                string url = $"{_configuration["ClientUrl"]}/alterarSenha?email={email}&token={TokenValido}";

                string bodyEmail = $@"<h1>Siga as instrucoes para alterar sua senha</h1>
                                      <p><a href='{url}'>Clique Aqui</a> para alterar sua Senha !</p>";

                await EnviarEmailAsync("Recuperacao de Senha", email, bodyEmail);
                return new Resposta
                {
                    Mensagem = "Email Enviado com Sucesso !",
                    Sucesso = true
                };
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

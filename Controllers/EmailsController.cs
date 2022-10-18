using BancoParaleloAPI.Data;
using BancoParaleloAPI.Hubs;
using BancoParaleloAPI.Interfaces;
using BancoParaleloAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace BancoParaleloAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly EmailService _email;
        private readonly UserService _userService;
        private readonly IHubContext<ConfirmarEmailHub> _hubContext;
        public EmailsController(AppDbContext context, EmailService email, UserService userService, IHubContext<ConfirmarEmailHub> hubContext)
        {
            _context = context;
            _email = email;
            _userService = userService;
            _hubContext = hubContext;
        }

        [HttpGet]
        public async Task<IActionResult> TesteEmail()
        {
            var email = "ncjuninho@hotmail.com";
            await _email.EnviarEmailAsync("Teste de envio de email", email, "Alterando email agora");
            return Ok();
        }

        // api/emails/enviarConfirmacaoEmail?email&signalrid
        [HttpPost("enviarConfirmacaoEmail")]
        public async Task<IActionResult> EnviarConfirmacaoEmail(string email, string signalrid)
        {
            try
            {
                if (email == "") return BadRequest("Email eh obrigatorio");

                var usuario = await _userService.RecuperarUsuarioPorEmail(email);
                await _email.EnviarConfirmacaoEmailAsync(usuario, signalrid);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Mensagem = ex.Message });
            }
        }

        // api/emails/emailEstaConfirmado
        [HttpGet("emailEstaConfirmado")]
        public async Task<IActionResult> EmailIsConfirmed(string email)
        {
            try
            {
                if(email == "")
                {
                    return BadRequest();
                }


                var usuario = await _context.Usuarios.FirstAsync(usuario => usuario.Email == email);
                if(usuario != null)
                {
                    return Ok(usuario.EmailConfirmado);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Mensagem = ex.Message });
            }
        }

        // api/emails/confirmarEmail?email&userid&token&signalrid
        [HttpPost("confirmarEmail")]
        public async Task<IActionResult> ConfirmarEmail(string email, string userId, string token, string signalrid)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
                {
                    return NotFound();
                }

                var retorno = await _email.ConfirmarEmailAsync(userId, token);
                if (retorno.Sucesso)
                {
                    var usuario = await _context.Usuarios.FirstAsync(usuario => usuario.Email == email);
                    usuario.EmailConfirmado = true;
                    _context.Usuarios.Update(usuario);
                    await _context.SaveChangesAsync();
                    await _hubContext.Clients.Client(signalrid).SendAsync("IsConfirmed", usuario.EmailConfirmado);
                    return Ok(retorno.Mensagem);
                }

                return BadRequest(retorno.Mensagem);

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Mensagem = ex.Message });
            }
        }

        // api/emails/recuperarSenha
        [HttpPost("recuperarSenha")]
        public async Task<IActionResult> EnviarRecuperacaoSenha(string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                    return NotFound();

                var emailCriptografado = Convert.ToBase64String(Encoding.Unicode.GetBytes(email));
                var usuario = await _context.Usuarios.FirstAsync(usuario => usuario.Email == emailCriptografado);
                var retorno = await _email.EnviarRecuperacaoSenhaAsync(email, usuario.Id.ToString());
                if (retorno.Sucesso)
                {
                    return Ok(retorno.Mensagem);
                }

                return BadRequest(retorno.Mensagem);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Mensagem = ex.Message });
            }
        }

        // api/emails/alterarSenha
        [HttpPost("alterarSenha")]
        public async Task<IActionResult> AlterarSenha(string email, string senha)
        {
            try
            {
                var emailCriptografado = Convert.ToBase64String(Encoding.Unicode.GetBytes(email));
                var usuario = await _context.Usuarios.FirstAsync(usuario => usuario.Email == emailCriptografado);

                usuario.Senha = senha;

                _context.Update(usuario);
                await _context.SaveChangesAsync();

                return Ok("Senha Alterada com Sucesso !");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Mensagem = ex.Message });
            }
        }

    }
}

using BancoParaleloAPI.Data;
using BancoParaleloAPI.Entidades;
using BancoParaleloAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BancoParaleloAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private IConfiguration _config;
        private AppDbContext _context;
        public AuthenticationController(IConfiguration config, AppDbContext context)
        {
            _config = config;
            _context = context;
        }

        [HttpPost, Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel login)
        {
            var usuario = await _context.Usuarios.FirstAsync(usuario => usuario.Email == login.Email);

            if (login == null)
            {
                return BadRequest("Request do cliente inválido");
            }
            if (login.Email == usuario.Email && login.Senha == usuario.Senha)
            {
                var _secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var _issuer = _config["Jwt:Issuer"];
                var _audience = _config["Jwt:Audience"];

                var signinCredentials = new SigningCredentials(_secretKey, SecurityAlgorithms.HmacSha256);

                var tokenOptions = new JwtSecurityToken(
                    issuer: _issuer,
                    audience: _audience,
                    claims: new List<Claim>(),
                    expires: DateTime.Now.AddMinutes(2),
                    signingCredentials: signinCredentials);

                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

                return Ok(new { Token = tokenString, StatusCode = 200, Usuario = usuario.FirstName });

            }
            else
            {
                return Unauthorized();
            }
        }

    }
}

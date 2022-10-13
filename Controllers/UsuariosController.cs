using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BancoParaleloAPI.Data;
using BancoParaleloAPI.Entidades;
using Microsoft.AspNetCore.Authorization;
using System.Text;
using BancoParaleloAPI.Data.DTO.Usuario;

namespace BancoParaleloAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Usuarios
        [HttpGet, Authorize]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            return await _context.Usuarios.ToListAsync();
        }

        // GET: api/Usuarios/5
        [HttpGet("{id}"), Authorize]
        public async Task<ActionResult<Usuario>> GetUsuario(uint id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }

        // PUT: api/Usuarios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}"), Authorize]
        public async Task<IActionResult> PutUsuario(uint id, Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return BadRequest();
            }

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(UsuarioDTO usuarioDTO)
        {
            try
            {
                uint idAgencia = usuarioDTO.agencia;
                uint idTipo = usuarioDTO.tipoDeConta;

                Agencia agencia = await _context.Agencias.FirstAsync(agencia => agencia.Id == idAgencia);
                TipoDeConta tipoConta = await _context.TiposDeConta.FirstAsync(tipo => tipo.Id == idTipo);

                List<Usuario> users = await _context.Usuarios.ToListAsync();
                List<Conta> contas = await _context.Contas.ToListAsync();
                int countContas = contas.Count();

                if(users.Where(user => user.Cpf == usuarioDTO.cpf).ToList().Count > 0)
                {
                    throw new Exception("Cpf ja cadastrado");
                }

                if(users.Where(user => user.Email == usuarioDTO.email).ToList().Count > 0)
                {
                    throw new Exception("Email ja cadastrado");
                }

                Endereco endereco = usuarioDTO.endereco;
                await _context.Enderecos.AddAsync(endereco);

                Usuario usuario = MapearUsuario(usuarioDTO);
                await _context.Usuarios.AddAsync(usuario);

                Conta conta = new Conta();
                conta.Agencia = agencia;
                conta.Usuario = usuario;
                conta.Saldo = 0;
                conta.Codigo = $"{AdicionarNumeralConta(countContas)}{countContas + 1}-{usuarioDTO.tipoDeConta}";
                conta.Tipo = tipoConta;

                await _context.Contas.AddAsync(conta);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetUsuario", new { id = usuario.Id }, usuario);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Mensagem = ex.Message });
            }
        }

        // DELETE: api/Usuarios/5
        [HttpDelete("{id}"), Authorize]
        public async Task<IActionResult> DeleteUsuario(uint id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsuarioExists(uint id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }

        private Endereco MapearEndereco(UsuarioDTO usuarioDTO)
        {
            Endereco endereco = new Endereco();
            endereco.CEP = usuarioDTO.endereco.CEP;
            endereco.Rua = usuarioDTO.endereco.Rua;
            endereco.Numero = usuarioDTO.endereco.Numero;
            endereco.Bairro = usuarioDTO.endereco.Bairro;
            endereco.Pais = usuarioDTO.endereco.Pais;
            endereco.Cidade = usuarioDTO.endereco.Cidade;
            endereco.Complemento = usuarioDTO.endereco.Complemento;
            endereco.Estado = usuarioDTO.endereco.Estado;
            return endereco;
        }

        private Usuario MapearUsuario(UsuarioDTO usuarioDTO)
        {
            Usuario usuario = new Usuario();
            usuario.FirstName = usuarioDTO.firstName;
            usuario.LastName = usuarioDTO.lastName;
            usuario.Email = usuarioDTO.email;
            usuario.Cpf = usuarioDTO.cpf;
            usuario.Telefone = usuarioDTO.telefone;
            usuario.Endereco = usuarioDTO.endereco;
            usuario.Senha = usuarioDTO.senha;
            return usuario;
        }

        private string AdicionarNumeralConta(int countConta)
        {
            if((countConta < 10 && countConta > 0) || countConta == 0)
            {
                return "000";
            }
            if(countConta < 100 && countConta > 9)
            {
                return "00";
            }
            if(countConta < 1000 && countConta > 99)
            {
                return "0";
            }

            return "";
        }
    }
}

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

namespace BancoParaleloAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransacoesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TransacoesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Transacoes
        [HttpGet, Authorize]
        public async Task<ActionResult<IEnumerable<Transacao>>> GetTransacoes()
        {
            return await _context.Transacoes.ToListAsync();
        }

        // GET: api/Transacoes/5
        [HttpGet("{id}"), Authorize]
        public async Task<ActionResult<Transacao>> GetTransacao(uint? id)
        {
            var transacao = await _context.Transacoes.FindAsync(id);

            if (transacao == null)
            {
                return NotFound();
            }

            return transacao;
        }

        // PUT: api/Transacoes/5
        [HttpPut("{id}"), Authorize]
        public async Task<IActionResult> PutTransacao(uint? id, Transacao transacao)
        {
            if (id != transacao.Id)
            {
                return BadRequest();
            }

            _context.Entry(transacao).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransacaoExists(id))
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

        // POST: api/Transacoes
        [HttpPost, Authorize]
        public async Task<ActionResult<Transacao>> PostTransacao(Transacao transacao)
        {
            _context.Transacoes.Add(transacao);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTransacao", new { id = transacao.Id }, transacao);
        }

        // DELETE: api/Transacoes/5
        [HttpDelete("{id}"), Authorize]
        public async Task<IActionResult> DeleteTransacao(uint? id)
        {
            var transacao = await _context.Transacoes.FindAsync(id);
            if (transacao == null)
            {
                return NotFound();
            }

            _context.Transacoes.Remove(transacao);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TransacaoExists(uint? id)
        {
            return _context.Transacoes.Any(e => e.Id == id);
        }
    }
}

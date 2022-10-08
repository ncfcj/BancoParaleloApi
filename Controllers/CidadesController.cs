using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BancoParaleloAPI.Data;
using BancoParaleloAPI.Entidades;

namespace BancoParaleloAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CidadesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CidadesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Cidades
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cidade>>> GetCidades()
        {
            return await _context.Cidades.ToListAsync();
        }

        // GET: api/Cidades/5
        [HttpGet("Estado/{idEstado}")]
        public async Task<ActionResult<List<Cidade>>> GetCidadePorEstado(uint idEstado)
        {
            var cidades = _context.Cidades;

            List<Cidade> cidadesFiltradas = await cidades.Where(x => x.UF == idEstado).ToListAsync();

            if (cidadesFiltradas == null)
            {
                return NotFound();
            }

            return cidadesFiltradas;
        }

        private bool CidadeExists(uint id)
        {
            return _context.Cidades.Any(e => e.id == id);
        }
    }
}

using BancoParaleloAPI.Data;
using BancoParaleloAPI.Entidades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BancoParaleloAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgenciasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AgenciasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Agencia>>> GetAgencias()
        {
            List<Agencia> agencias = await _context.Agencias.ToListAsync();
            if(agencias.Count != 0)
            {
                return Ok(agencias);
            }
            return NotFound();
        }
    }
}

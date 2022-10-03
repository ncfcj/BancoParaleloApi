using BancoParaleloAPI.Entidades;
using Microsoft.EntityFrameworkCore;

namespace BancoParaleloAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {

        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Transacao> Transacoes { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
    }
}

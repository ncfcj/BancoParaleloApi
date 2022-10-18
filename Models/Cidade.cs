using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BancoParaleloAPI.Entidades
{
    public class Cidade
    {
        [Key]
        public uint id { get; set; }
        [Column("nome")]
        public string Nome { get; set; }
        [Column("uf")]
        public uint UF { get; set; }
        [Column("ibge")]
        public uint IBGE { get; set; }
    }
}

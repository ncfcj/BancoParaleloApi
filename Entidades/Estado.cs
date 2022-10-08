using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BancoParaleloAPI.Entidades
{
    public class Estado
    {
        [Key]
        public int id { get; set; }

        [Column("nome")]
        public string Nome { get; set; }

        [Column("uf")]
        public string UF { get; set; }

        [Column("ibge")]
        public int IBGE { get; set; }

        [Column("pais")]
        public int Pais { get; set; }

        [Column("ddd")]
        public string DDD { get; set; }
    }
}

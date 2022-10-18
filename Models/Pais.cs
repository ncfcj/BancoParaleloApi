using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BancoParaleloAPI.Entidades
{
    public class Pais
    {
        [Key]
        public int id { get; set; }

        [Column("nome")]
        public string Nome { get; set; }
        [Column("nome_pt")]
        public string NomeEmPortugues { get; set; }

        [Column("sigla")]
        public string Sigla { get; set; }

        [Column("bacen")]
        public int Bacen { get; set; }

    }
}

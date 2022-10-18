using System.ComponentModel.DataAnnotations;

namespace BancoParaleloAPI.Entidades
{
    public class Conta
    {
        [Key]
        public uint? Id { get; set; }
        public string? Codigo { get; set; }
        public Agencia? Agencia { get; set; }
        public decimal? Saldo { get; set; }
        public TipoDeConta? Tipo { get; set; }
        public Usuario? Usuario { get; set; }
    }
}

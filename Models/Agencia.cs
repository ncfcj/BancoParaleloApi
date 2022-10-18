using System.ComponentModel.DataAnnotations;

namespace BancoParaleloAPI.Entidades
{
    public class Agencia
    {
        [Key]
        public uint? Id { get; set; }
        public string? Codigo { get; set; }
        public string? Nome { get; set; }
    }
}

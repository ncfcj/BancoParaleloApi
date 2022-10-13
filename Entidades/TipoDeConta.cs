using System.ComponentModel.DataAnnotations;

namespace BancoParaleloAPI.Entidades
{
    public class TipoDeConta
    {
        [Key]
        public uint? Id { get; set; }
        public string? Tipo { get; set; }
    }
}

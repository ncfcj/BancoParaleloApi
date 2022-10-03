using System.ComponentModel.DataAnnotations;

namespace BancoParaleloAPI.Entidades
{
    public class Endereco
    {
        [Key]
        public uint Id { get; set; }
        public string? CEP { get; set; }
        public string? Rua { get; set; }
        public uint Numero { get; set; }
        public string? Complemento { get; set; }
        public string? Bairro { get; set; }
        public string? Cidade { get; set; }
        public string? Estado { get; set; }
        public string? Pais { get; set; }
    }
}

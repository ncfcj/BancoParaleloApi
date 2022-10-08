using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BancoParaleloAPI.Entidades
{
    public class Usuario
    {
        [Key]
        public uint Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Cpf { get; set; }
        public string? Email { get; set; }
        public string? Telefone { get; set; }
        public string? Username { get; set; }
        public string? Senha { get; set; }
        public Endereco? Endereco { get; set; }
        public uint? TipoDeConta { get; set; }
    }
}

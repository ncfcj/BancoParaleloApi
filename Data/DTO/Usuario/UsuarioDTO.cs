using BancoParaleloAPI.Entidades;

namespace BancoParaleloAPI.Data.DTO.Usuario
{
    public class UsuarioDTO
    {
        public string email { get; set; }
        public string senha { get; set; }
        public string cpf { get; set; }
        public Endereco endereco { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string telefone { get; set; }
        public uint tipoDeConta { get; set; }
        public uint agencia { get; set; }
    }
}

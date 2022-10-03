namespace BancoParaleloAPI.Entidades
{
    public class Transacao
    {
        public uint? Id { get; set; }
        public float? Valor { get; set; }
        public string? ContaOrigem { get; set; }
        public string? ContaDestino { get; set; }
    }
}

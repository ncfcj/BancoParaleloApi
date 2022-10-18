namespace BancoParaleloAPI.Interfaces
{
    public interface IConfirmarEmailHub
    {
        Task DisplayMessage(string message);
        Task IsConfirmed(bool emailConfirmado);
    }
}

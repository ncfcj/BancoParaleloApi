using BancoParaleloAPI.Data;
using BancoParaleloAPI.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using NuGet.Common;
using System.Diagnostics;
using System.Text;

namespace BancoParaleloAPI.Hubs
{
    public class ConfirmarEmailHub : Hub<IConfirmarEmailHub>
    {
        protected IHubContext<ConfirmarEmailHub> _context;
        public ConfirmarEmailHub(IHubContext<ConfirmarEmailHub> context)
        {
            _context = context;
        }

        public void Hello()
        {
            Clients.Caller.DisplayMessage("Hello from the ConfirmarEmailHub");
        }

        public async Task IsConfirmed(bool emailConfirmado)
        {          
            await Clients.Caller.IsConfirmed(emailConfirmado);
        }
    }
}

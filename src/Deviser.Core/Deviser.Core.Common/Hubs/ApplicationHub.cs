using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Deviser.Core.Common.Hubs
{
    public class ApplicationHub : Hub
    {
        //public async Task SendMessage(string message)
        //{
        //    await Clients.All.SendAsync("OnStarted", message);
        //}   
        public async Task UpdateInstallLog(string message)
        {
            await Clients.All.SendAsync("OnUpdateInstallLog", message);
        }
    }
}

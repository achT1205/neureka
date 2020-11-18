using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace NeurekaApi.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

    }
}

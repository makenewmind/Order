using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Order.API.Hubs
{
    public class OrderHub : Hub
    {
        public async Task SendOrderCreated(string orderNumber, string details)
        {
            await Clients.All.SendAsync("ReceiveOrderCreated", orderNumber, details);
        }

        public async Task SendOrderStatusChanged(int orderId, string status)
        {
            await Clients.All.SendAsync("ReceiveOrderStatusChanged", orderId, status);
        }

        public async Task SendBillUpdated(int orderId, decimal amount)
        {
            await Clients.All.SendAsync("ReceiveBillUpdated", orderId, amount);
        }

        public async Task SendItemCompleted(int orderId, int itemId)
        {
            await Clients.All.SendAsync("ReceiveItemCompleted", orderId, itemId);
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            await Clients.Caller.SendAsync("ReceiveMessage", "Connected to Order Hub");
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}
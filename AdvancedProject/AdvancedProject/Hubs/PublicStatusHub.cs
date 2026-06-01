using Microsoft.AspNetCore.SignalR;

namespace AdvancedProject.Hubs
{
    public class PublicStatusHub : Hub
    {
        public async Task JoinRequestGroup(int requestId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"request-{requestId}");
        }
    }
}

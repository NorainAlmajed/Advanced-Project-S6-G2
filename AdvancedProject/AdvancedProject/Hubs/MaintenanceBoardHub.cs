using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace AdvancedProject.Hubs
{
    [Authorize]
    public class MaintenanceBoardHub : Hub
    {
        // Every authenticated user who opens the maintenance board page joins this group.
        // The controller broadcasts "BoardUpdated" to the group whenever a request is
        // created or its status/assignment changes.
        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "maintenance-board");
            await base.OnConnectedAsync();
        }
    }
}

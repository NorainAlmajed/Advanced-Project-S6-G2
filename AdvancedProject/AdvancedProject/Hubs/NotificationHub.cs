using AdvancedProjectAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace AdvancedProject.Hubs
{
    [Authorize]
    public class NotificationHub : Hub
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public NotificationHub(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // When a user connects, add them to a personal group keyed by their custom integer UserId.
        // The controller pushes to "user-{customUserId}" so the right person gets the notification.
        public override async Task OnConnectedAsync()
        {
            var identityUser = await _userManager.GetUserAsync(Context.User);
            if (identityUser?.UserId != null)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"user-{identityUser.UserId}");
            }
            await base.OnConnectedAsync();
        }
    }
}

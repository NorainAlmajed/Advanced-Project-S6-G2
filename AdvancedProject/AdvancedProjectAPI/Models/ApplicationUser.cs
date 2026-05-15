using Microsoft.AspNetCore.Identity;

namespace AdvancedProjectAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public int? UserId { get; set; }
    }
}
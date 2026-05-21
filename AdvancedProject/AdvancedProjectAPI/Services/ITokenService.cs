using AdvancedProjectAPI.Models;

namespace AdvancedProjectAPI.Services
{
    public interface ITokenService
    {
        Task<string> CreateTokenAsync(ApplicationUser user);
    }
}

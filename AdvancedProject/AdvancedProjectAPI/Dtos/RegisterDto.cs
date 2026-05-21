using System.ComponentModel.DataAnnotations;

namespace AdvancedProjectAPI.Dtos;

public class RegisterDto
{
    [Required, EmailAddress]
    public required string Email { get; set; }

    [Required, StringLength(50, MinimumLength = 2)]
    public required string FullName { get; set; }  // Changed from DisplayName

    [Required, StringLength(100, MinimumLength = 6)]
    public required string Password { get; set; }
}
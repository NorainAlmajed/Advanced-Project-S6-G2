using System.ComponentModel.DataAnnotations;

namespace AdvancedProject.Models
{
    public class TenantCreateVM
    {
        // User fields
        [Required]
        public string Username { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

        [Required]
        public string FullName { get; set; } = null!;

        [Required]
        public string Email { get; set; } = null!;

        [Required]
        public string Phone { get; set; } = null!;

        [Required]
        public string? Gender { get; set; }

        // Tenant fields
        [Required]
        public DateTime Dob { get; set; } = DateTime.Today;

        [Required]
        public string NationalId { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace AdvancedProject.Models
{
    public class TenantEditVM
    {
        public int TenantId { get; set; }
        public int UserId { get; set; }

        [Required]
        public string Username { get; set; }

        public string? Password { get; set; } // optional

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public DateTime Dob { get; set; }

        [Required]
        public string NationalId { get; set; }
    }
}

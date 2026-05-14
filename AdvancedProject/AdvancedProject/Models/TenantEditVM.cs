using System.ComponentModel.DataAnnotations;

namespace AdvancedProject.Models
{
    public class TenantEditVM
    {
        public int TenantId { get; set; }
        public int UserId { get; set; }

        [Required]
        public string Username { get; set; } = null!;

        public string? Password { get; set; } // optional

        [Required]
        public string FullName { get; set; } = null!;

        [Required]
        public string Email { get; set; } = null!;

        [Required]
        public string Phone { get; set; } = null!;

        [Required]
        public string Gender { get; set; } = null!;

        [Required]
        public DateTime Dob { get; set; }

        [Required]
        public string NationalId { get; set; } = null!;

        [Required(ErrorMessage = "Salary is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Salary must be a valid positive number.")]
        public decimal? Salary { get; set; }

        // No [Required] — always has a value (defaults to "Undetermined")
        public string FinancialStability { get; set; } = "Undetermined";

        [Required(ErrorMessage = "Marital Status is required.")]
        public string? MaritalStatus { get; set; }

        [Required(ErrorMessage = "Employment Status is required.")]
        public string? EmploymentStatus { get; set; }
    }
}
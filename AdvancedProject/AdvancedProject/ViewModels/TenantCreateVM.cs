using System.ComponentModel.DataAnnotations;
namespace AdvancedProject.ViewModels
{
    public class TenantCreateVM
    {
        [Required]
        public string Username { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Please confirm your password.")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } = null!;

        [Required]
        public string FullName { get; set; } = null!;

        [Required]
        public string Email { get; set; } = null!;

        [Required]
        public string Phone { get; set; } = null!;

        [Required]
        public string? Gender { get; set; }

        [Required(ErrorMessage = "Date of birth is required.")]
        public DateTime? Dob { get; set; }

        [Required]
        public string NationalId { get; set; } = null!;

        [Required(ErrorMessage = "Salary is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Salary must be a valid positive number.")]
        public decimal? Salary { get; set; }

        [Required(ErrorMessage = "Marital Status is required.")]
        public string? MaritalStatus { get; set; }

        [Required(ErrorMessage = "Employment Status is required.")]
        public string? EmploymentStatus { get; set; }
    }
}
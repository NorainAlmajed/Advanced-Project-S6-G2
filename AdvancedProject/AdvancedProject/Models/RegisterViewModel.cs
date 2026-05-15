using System.ComponentModel.DataAnnotations;

namespace AdvancedProject.Models
{
    public class RegisterViewModel
    {
        [Required]
        public string Username { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 8)]
        public string Password { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } = null!;

        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string Phone { get; set; } = null!;

        [Required]
        public string Gender { get; set; } = null!;

        [Required]
        [Display(Name = "Date of Birth")]
        public DateTime Dob { get; set; } = DateTime.Today;

        [Required]
        [Display(Name = "National ID")]
        public string NationalId { get; set; } = null!;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Salary must be a valid positive number.")]
        public decimal? Salary { get; set; }

        [Required(ErrorMessage = "Marital Status is required.")]
        [Display(Name = "Marital Status")]
        public string? MaritalStatus { get; set; }

        [Required(ErrorMessage = "Employment Status is required.")]
        [Display(Name = "Employment Status")]
        public string? EmploymentStatus { get; set; }
    }
}
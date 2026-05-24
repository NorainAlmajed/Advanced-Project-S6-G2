using System.ComponentModel.DataAnnotations;
namespace AdvancedProject.ViewModels
{
    public class TenantEditVM : IValidatableObject
    {
        public int TenantId { get; set; }
        public int UserId { get; set; }

        [Required]
        public string Username { get; set; } = null!;

        public string? Password { get; set; }

        public string? ConfirmPassword { get; set; }

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

        public string FinancialStability { get; set; } = "Undetermined";

        [Required(ErrorMessage = "Marital Status is required.")]
        public string? MaritalStatus { get; set; }

        [Required(ErrorMessage = "Employment Status is required.")]
        public string? EmploymentStatus { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrWhiteSpace(Password))
            {
                if (Password.Length < 8)
                    yield return new ValidationResult(
                        "Password must be at least 8 characters.",
                        new[] { nameof(Password) });

                else if (!System.Text.RegularExpressions.Regex.IsMatch(Password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).+$"))
                    yield return new ValidationResult(
                        "Password must include uppercase, lowercase, a number, and a special character.",
                        new[] { nameof(Password) });

                if (string.IsNullOrWhiteSpace(ConfirmPassword))
                    yield return new ValidationResult(
                        "Please confirm your new password.",
                        new[] { nameof(ConfirmPassword) });
                else if (Password != ConfirmPassword)
                    yield return new ValidationResult(
                        "Passwords do not match.",
                        new[] { nameof(ConfirmPassword) });
            }
        }
    }
}
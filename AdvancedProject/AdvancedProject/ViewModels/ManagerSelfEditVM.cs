using System.ComponentModel.DataAnnotations;

namespace AdvancedProject.ViewModels
{
    public class ManagerSelfEditVM : IValidatableObject
    {
        public int ManagerId { get; set; }
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

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrWhiteSpace(Password))
            {
                if (string.IsNullOrWhiteSpace(ConfirmPassword))
                    yield return new ValidationResult("Please confirm your new password.", new[] { nameof(ConfirmPassword) });
                else if (Password != ConfirmPassword)
                    yield return new ValidationResult("Passwords do not match.", new[] { nameof(ConfirmPassword) });
            }
        }
    }
}

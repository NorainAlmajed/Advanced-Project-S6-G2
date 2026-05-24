using System.ComponentModel.DataAnnotations;
namespace AdvancedProject.ViewModels
{
    public class MaintenanceStaffEditVM : IValidatableObject
    {
        public int StaffId { get; set; }
        public int UserId { get; set; }

        public string Username { get; set; }

        public string? Password { get; set; }

        public string? ConfirmPassword { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Gender { get; set; }

        public string AvailabilityStatus { get; set; }

        public List<int> SelectedSkillIds { get; set; } = new();

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
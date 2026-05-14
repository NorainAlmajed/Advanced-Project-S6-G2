using System.ComponentModel.DataAnnotations;
namespace AdvancedProject.Models
{
    public class MaintenanceStaffCreateVM
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please confirm your password.")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Gender { get; set; }

        public List<int> SelectedSkillIds { get; set; } = new List<int>();
    }
}
using System.ComponentModel.DataAnnotations;

namespace AdvancedProject.Models
{
    public class MaintenanceStaffCreateVM
    {
        // User fields
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Gender { get; set; }

        // Staff fields
        public List<int> SelectedSkillIds { get; set; } = new List<int>();
    }
}

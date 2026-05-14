using System.ComponentModel.DataAnnotations;

namespace AdvancedProject.Models
{
    public class MaintenanceStaffEditVM
    {
        public int StaffId { get; set; }
        public int UserId { get; set; }

        // User fields
        public string Username { get; set; }
        public string? Password { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; }

        // Staff fields
        public string AvailabilityStatus { get; set; }

        // Skills
        public List<int> SelectedSkillIds { get; set; } = new();
    }
}

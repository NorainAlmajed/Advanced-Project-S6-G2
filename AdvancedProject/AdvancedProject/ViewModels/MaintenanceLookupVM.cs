using System.ComponentModel.DataAnnotations;

namespace AdvancedProject.ViewModels
{
    public class MaintenanceLookupVM
    {
        [Required(ErrorMessage = "Ticket number is required.")]
        [Display(Name = "Ticket Number")]
        public int? TicketNumber { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; } = "";

        // Result — null means no search yet or not found
        public MaintenanceResultVM? Result { get; set; }

        // true = searched but nothing found
        public bool NotFound { get; set; }
    }

    public class MaintenanceResultVM
    {
        public int RequestId { get; set; }
        public string Status { get; set; } = "";
        public string Priority { get; set; } = "";
        public string? Notes { get; set; }
        public DateTime RequestDate { get; set; }
        public string UnitNumber { get; set; } = "";
        public string SkillName { get; set; } = "";
        public string? AssignedStaffName { get; set; }
    }
}

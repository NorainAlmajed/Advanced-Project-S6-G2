namespace AdvancedProjectAPI.Dtos
{
    public class MaintenanceRequestDto
    {
        public int RequestId { get; set; }
        public string Status { get; set; } = null!;
        public string Priority { get; set; } = null!;
        public string Notes { get; set; } = null!;
        public DateTime RequestDate { get; set; }
        public string UnitNumber { get; set; } = null!;
        public string SkillName { get; set; } = null!;
        public string? AssignedStaffName { get; set; }
    }
}
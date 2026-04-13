using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AdvancedProject.Models;

[Table("MaintenanceStaff")]
[Index("UserId", Name = "IX_MaintenanceStaff_UserId")]
public partial class MaintenanceStaff
{
    [Key]
    public int StaffId { get; set; }

    [StringLength(20)]
    public string AvailabilityStatus { get; set; } = null!;

    public int UserId { get; set; }

    [InverseProperty("AssignedStaff")]
    public virtual ICollection<MaintenanceRequest> MaintenanceRequests { get; set; } = new List<MaintenanceRequest>();

    [ForeignKey("UserId")]
    [InverseProperty("MaintenanceStaffs")]
    public virtual User User { get; set; } = null!;

    [ForeignKey("StaffId")]
    [InverseProperty("Staff")]
    public virtual ICollection<Skill> Skills { get; set; } = new List<Skill>();
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AdvancedProject.Models;

[Table("MaintenanceStaff")]
public partial class MaintenanceStaff
{
    [Key]
    public int StaffId { get; set; }

    [StringLength(20)]
    public string AvailabilityStatus { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public int UserId { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("MaintenanceStaffs")]
    public virtual User User { get; set; } = null!;

    [ForeignKey("StaffId")]
    [InverseProperty("Staff")]
    public virtual ICollection<Skill> Skills { get; set; } = new List<Skill>();
}

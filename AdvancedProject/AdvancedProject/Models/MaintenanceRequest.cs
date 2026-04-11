using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AdvancedProject.Models;

public partial class MaintenanceRequest
{
    [Key]
    public int RequestId { get; set; }

    public int UnitId { get; set; }

    public int TenantId { get; set; }

    public DateTime RequestDate { get; set; }

    public int SkillId { get; set; }

    [StringLength(20)]
    public string Priority { get; set; } = null!;

    [StringLength(20)]
    public string Status { get; set; } = null!;

    public int? AssignedStaffId { get; set; }

    [StringLength(500)]
    public string? Notes { get; set; }

    public DateTime? CompletedDate { get; set; }

    [ForeignKey("AssignedStaffId")]
    [InverseProperty("MaintenanceRequests")]
    public virtual User? AssignedStaff { get; set; }

    [ForeignKey("SkillId")]
    [InverseProperty("MaintenanceRequests")]
    public virtual Skill Skill { get; set; } = null!;

    [ForeignKey("TenantId")]
    [InverseProperty("MaintenanceRequests")]
    public virtual Tenant Tenant { get; set; } = null!;

    [ForeignKey("UnitId")]
    [InverseProperty("MaintenanceRequests")]
    public virtual Unit Unit { get; set; } = null!;
}

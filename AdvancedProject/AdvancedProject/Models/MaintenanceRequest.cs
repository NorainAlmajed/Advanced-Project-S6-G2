using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdvancedProject.Models;

[Index("AssignedStaffId", Name = "IX_MaintenanceRequests_AssignedStaffId")]
[Index("SkillId", Name = "IX_MaintenanceRequests_SkillId")]
[Index("UnitId", Name = "IX_MaintenanceRequests_UnitId")]
public partial class MaintenanceRequest
{
    [Key]
    public int RequestId { get; set; }

    [Display(Name = "Unit Number")]
    public int UnitId { get; set; }

    public int UserId { get; set; }

    [ForeignKey("UserId")]
    [ValidateNever]
    public virtual User User { get; set; } = null!;

    public DateTime RequestDate { get; set; }

    [Display(Name = " Maintenance Type")]
    public int SkillId { get; set; }

    [StringLength(20)]
    public string Priority { get; set; } = null!;

    [StringLength(20)]
    public string Status { get; set; } = "Pending";

    public int? AssignedStaffId { get; set; }

    [StringLength(500)]
    [Display(Name = "Description")]
    public string? Notes { get; set; }

    public DateTime? CompletedDate { get; set; }

    public DateTime? AssignedTime { get; set; }

    public DateTime? ResolvedTime { get; set; }

    public DateTime? ClosedTime { get; set; }

    public DateTime? InProgressTime { get; set; }

    [ValidateNever]
    [ForeignKey("AssignedStaffId")]
    [InverseProperty("MaintenanceRequests")]
    public virtual MaintenanceStaff? AssignedStaff { get; set; }

    [ValidateNever]
    [ForeignKey("SkillId")]
    [InverseProperty("MaintenanceRequests")]
    public virtual Skill Skill { get; set; } = null!;


    [ValidateNever]
    [ForeignKey("UnitId")]
    [InverseProperty("MaintenanceRequests")]
    public virtual Unit Unit { get; set; } = null!;
}

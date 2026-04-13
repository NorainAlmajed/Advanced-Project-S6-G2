using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AdvancedProject.Models;

[Index("TenantId", Name = "IX_LeaseApplications_TenantId")]
[Index("UnitId", Name = "IX_LeaseApplications_UnitId")]
public partial class LeaseApplication
{
    [Key]
    public int ApplicationId { get; set; }

    public int TenantId { get; set; }

    public int UnitId { get; set; }

    public DateTime ApplicationDate { get; set; }

    [StringLength(20)]
    public string Status { get; set; } = null!;

    public DateTime? ApproveTime { get; set; }

    public DateTime? RejectTime { get; set; }

    public DateTime StartDate { get; set; }

    public int Duration { get; set; }

    [ForeignKey("TenantId")]
    [InverseProperty("LeaseApplications")]
    public virtual Tenant Tenant { get; set; } = null!;

    [ForeignKey("UnitId")]
    [InverseProperty("LeaseApplications")]
    public virtual Unit Unit { get; set; } = null!;
}

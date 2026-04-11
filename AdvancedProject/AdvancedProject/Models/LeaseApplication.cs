using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AdvancedProject.Models;

public partial class LeaseApplication
{
    [Key]
    public int ApplicationId { get; set; }

    public int TenantId { get; set; }

    public int UnitId { get; set; }

    public DateTime ApplicationDate { get; set; }

    [StringLength(20)]
    public string Status { get; set; } = null!;

    [StringLength(500)]
    public string? Notes { get; set; }

    [ForeignKey("TenantId")]
    [InverseProperty("LeaseApplications")]
    public virtual Tenant Tenant { get; set; } = null!;

    [ForeignKey("UnitId")]
    [InverseProperty("LeaseApplications")]
    public virtual Unit Unit { get; set; } = null!;
}

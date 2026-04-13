using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AdvancedProject.Models;

[Index("TenantId", Name = "IX_Leases_TenantId")]
[Index("UnitId", Name = "IX_Leases_UnitId")]
public partial class Lease
{
    [Key]
    public int LeaseId { get; set; }

    public int TenantId { get; set; }

    public int UnitId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal MonthlyRent { get; set; }

    [StringLength(20)]
    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? TerminationDate { get; set; }

    [InverseProperty("Lease")]
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    [ForeignKey("TenantId")]
    [InverseProperty("Leases")]
    public virtual Tenant Tenant { get; set; } = null!;

    [ForeignKey("UnitId")]
    [InverseProperty("Leases")]
    public virtual Unit Unit { get; set; } = null!;
}

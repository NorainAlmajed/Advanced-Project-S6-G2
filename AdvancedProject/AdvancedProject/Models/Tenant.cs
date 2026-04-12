using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AdvancedProject.Models;

public partial class Tenant
{
    [Key]
    public int TenantId { get; set; }

    [Column("DOB")]
    public DateOnly? Dob { get; set; }

    [StringLength(20)]
    public string? NationalId { get; set; }

    [StringLength(100)]
    public string? EmergencyContact { get; set; }

    public DateTime CreatedAt { get; set; }

    public int UserId { get; set; }

    [InverseProperty("Tenant")]
    public virtual ICollection<LeaseApplication> LeaseApplications { get; set; } = new List<LeaseApplication>();

    [InverseProperty("Tenant")]
    public virtual ICollection<Lease> Leases { get; set; } = new List<Lease>();

    [InverseProperty("Tenant")]
    public virtual ICollection<MaintenanceRequest> MaintenanceRequests { get; set; } = new List<MaintenanceRequest>();

    [ForeignKey("UserId")]
    [InverseProperty("Tenants")]
    public virtual User User { get; set; } = null!;
}

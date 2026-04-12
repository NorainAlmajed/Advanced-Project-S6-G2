using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AdvancedProject.Models;

public partial class Unit
{
    [Key]
    public int UnitId { get; set; }

    public int PropertyId { get; set; }

    [StringLength(20)]
    public string UnitNumber { get; set; } = null!;

    [StringLength(50)]
    public string Type { get; set; } = null!;

    [Column(TypeName = "decimal(10, 0)")]
    public decimal? SizeSqFt { get; set; }

    [StringLength(255)]
    public string? Amenities { get; set; }

    [Column(TypeName = "decimal(10, 0)")]
    public decimal RentAmount { get; set; }

    [StringLength(20)]
    public string AvailabilityStatus { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    [InverseProperty("Unit")]
    public virtual ICollection<LeaseApplication> LeaseApplications { get; set; } = new List<LeaseApplication>();

    [InverseProperty("Unit")]
    public virtual ICollection<Lease> Leases { get; set; } = new List<Lease>();

    [InverseProperty("Unit")]
    public virtual ICollection<MaintenanceRequest> MaintenanceRequests { get; set; } = new List<MaintenanceRequest>();

    [ForeignKey("PropertyId")]
    [InverseProperty("Units")]
    public virtual Property Property { get; set; } = null!;
}

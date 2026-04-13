using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace AdvancedProject.Models;

[Index("PropertyId", Name = "IX_Units_PropertyId")]
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
    public decimal SizeSqFt { get; set; }

    [Column(TypeName = "decimal(10, 0)")]
    public decimal RentAmount { get; set; }

    [StringLength(20)]
    public string AvailabilityStatus { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    [ValidateNever]
    [InverseProperty("Unit")]
    public virtual ICollection<LeaseApplication> LeaseApplications { get; set; } = new List<LeaseApplication>();

    [ValidateNever]
    [InverseProperty("Unit")]
    public virtual ICollection<Lease> Leases { get; set; } = new List<Lease>();

    [ValidateNever]
    [InverseProperty("Unit")]
    public virtual ICollection<MaintenanceRequest> MaintenanceRequests { get; set; } = new List<MaintenanceRequest>();

    [ValidateNever]
    [ForeignKey("PropertyId")]
    [InverseProperty("Units")]
    public virtual Property Property { get; set; } = null!;

    [ValidateNever]
    [ForeignKey("UnitId")]
    [InverseProperty("Units")]
    public virtual ICollection<Amenity> Amenities { get; set; } = new List<Amenity>();
}

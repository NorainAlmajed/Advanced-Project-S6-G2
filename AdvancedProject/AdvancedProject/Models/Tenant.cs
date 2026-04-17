using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;

namespace AdvancedProject.Models;

[Index("UserId", Name = "IX_Tenants_UserId")]
public partial class Tenant
{
    [Key]
    public int TenantId { get; set; }

    [Required]
    [Column("DOB")]
    public DateOnly Dob { get; set; }

    [Required]
    [StringLength(20)]
    public string NationalId { get; set; } = null!;

    public int UserId { get; set; }

    [InverseProperty("Tenant")]
    public virtual ICollection<LeaseApplication> LeaseApplications { get; set; } = new List<LeaseApplication>();

    [InverseProperty("Tenant")]
    public virtual ICollection<Lease> Leases { get; set; } = new List<Lease>();

    [ForeignKey("UserId")]
    [InverseProperty("Tenants")]
    [ValidateNever]
    public virtual User User { get; set; } = null!;
}

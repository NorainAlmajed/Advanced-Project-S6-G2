using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdvancedProject.Models;

public partial class Property
{
    [Key]
    public int PropertyId { get; set; }

    [StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(50)]
    public string City { get; set; } = null!;

    [StringLength(500)]
    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    [StringLength(50)]
    public string Block { get; set; } = null!;

    [StringLength(50)]
    public string Building { get; set; } = null!;

    [StringLength(50)]
    public string Road { get; set; } = null!;

    public bool IsActive { get; set; } = true;

    [InverseProperty("Property")]
    public virtual ICollection<Unit> Units { get; set; } = new List<Unit>();

    public int GovernorateId { get; set; }

    [ForeignKey("GovernorateId")]
    [ValidateNever]
    public virtual Governorate Governorate { get; set; } = null!;
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AdvancedProject.Models;

public partial class Property
{
    [Key]
    public int PropertyId { get; set; }

    [StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(50)]
    public string Building { get; set; } = null!;

    [StringLength(50)]
    public string Road { get; set; } = null!;

    [StringLength(50)]
    public string Block { get; set; } = null!;

    [StringLength(50)]
    public string? Floor { get; set; }

    [StringLength(50)]
    public string City { get; set; } = null!;

    [StringLength(500)]
    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    [InverseProperty("Property")]
    public virtual ICollection<Unit> Units { get; set; } = new List<Unit>();
}

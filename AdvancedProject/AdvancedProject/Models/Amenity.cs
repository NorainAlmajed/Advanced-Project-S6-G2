using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AdvancedProject.Models;

public partial class Amenity
{
    [Key]
    public int AmenityId { get; set; }

    [StringLength(50)]
    public string Name { get; set; } = null!;

    [ForeignKey("AmenityId")]
    [InverseProperty("Amenities")]
    public virtual ICollection<Unit> Units { get; set; } = new List<Unit>();
}

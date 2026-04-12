using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AdvancedProject.Models;

public partial class PropertyManager
{
    [Key]
    public int ManagerId { get; set; }

    public int UserId { get; set; }

    public DateOnly HireDate { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("PropertyManagers")]
    public virtual User User { get; set; } = null!;
}

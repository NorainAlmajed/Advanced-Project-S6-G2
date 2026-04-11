using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AdvancedProject.Models;

public partial class Payment
{
    [Key]
    public int PaymentId { get; set; }

    public int LeaseId { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Amount { get; set; }

    public DateTime PaymentDate { get; set; }

    [StringLength(20)]
    public string Status { get; set; } = null!;

    [StringLength(500)]
    public string? Notes { get; set; }

    [ForeignKey("LeaseId")]
    [InverseProperty("Payments")]
    public virtual Lease Lease { get; set; } = null!;
}

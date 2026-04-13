using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AdvancedProject.Models;

[Index("LeaseId", Name = "IX_Payments_LeaseId")]
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

    [ForeignKey("LeaseId")]
    [InverseProperty("Payments")]
    public virtual Lease Lease { get; set; } = null!;
}

using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdvancedProject.Models;

[Index("LeaseId", Name = "IX_Payments_LeaseId")]
public partial class Payment
{
    [Key]
    public int PaymentId { get; set; }

    [Display(Name = "Lease Id")]
    public int LeaseId { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Amount { get; set; }


    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    [StringLength(20)]
    public string Status { get; set; } = null!;

    [ForeignKey("LeaseId")]
    [InverseProperty("Payments")]
    [ValidateNever]
    public virtual Lease Lease { get; set; } = null!;

    [Display(Name = "Payment Method")]
    public int PaymentMethodId { get; set; }

    [ForeignKey("PaymentMethodId")]
    [ValidateNever]
    public virtual PaymentMethod PaymentMethod { get; set; } = null!;

    [Display(Name = "Payment Frequency")]
    public int PaymentFrequencyId { get; set; }

    [ForeignKey("PaymentFrequencyId")]
    [ValidateNever]
    public virtual PaymentFrequency PaymentFrequency { get; set; } = null!;


}

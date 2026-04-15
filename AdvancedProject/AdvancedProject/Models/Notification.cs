using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AdvancedProject.Models;

[Index("UserId", Name = "IX_Notifications_UserId")]
public partial class Notification
{
    [Key]
    public int NotificationId { get; set; }

    public int UserId { get; set; }

    [StringLength(500)]
    public string Message { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    [StringLength(100)]
    public string Title { get; set; } = null!;

    public int NotificationTypeId { get; set; }

    [ForeignKey("NotificationTypeId")]
    public virtual NotificationType NotificationType { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("Notifications")]
    public virtual User User { get; set; } = null!;
}

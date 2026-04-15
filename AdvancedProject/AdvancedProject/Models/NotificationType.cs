using System.ComponentModel.DataAnnotations;

namespace AdvancedProject.Models
{
    public class NotificationType
    {
        [Key]
        public int NotificationTypeId { get; set; }

        [StringLength(50)]
        public string Name { get; set; } = null!;


        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}

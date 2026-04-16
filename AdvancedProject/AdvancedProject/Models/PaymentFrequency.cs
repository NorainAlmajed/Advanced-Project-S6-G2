using System.ComponentModel.DataAnnotations;

namespace AdvancedProject.Models
{
    public class PaymentFrequency
    {
        [Key]
        public int PaymentFrequencyId { get; set; }

        // number of months (1, 3, 6, 12)
        public int Frequency { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = null!;

        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}

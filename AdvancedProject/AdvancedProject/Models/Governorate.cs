using System.ComponentModel.DataAnnotations;

namespace AdvancedProject.Models
{
    public class Governorate
    {
        [Key]
        public int GovernorateId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;

        // navigation
        public virtual ICollection<Property> Properties { get; set; } = new List<Property>();
    }
}

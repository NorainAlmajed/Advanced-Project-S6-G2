using System.ComponentModel.DataAnnotations;

namespace AdvancedProject.Models
{
    public class UnitType
    {
        [Key]
        public int UnitTypeId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = null!;

        public virtual ICollection<Unit> Units { get; set; } = new List<Unit>();
    }
}

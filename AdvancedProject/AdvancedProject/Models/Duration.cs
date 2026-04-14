namespace AdvancedProject.Models
{
    public class Duration
    {
        public int DurationId { get; set; }

        public int Months { get; set; } // 6, 12, 24

        public ICollection<LeaseApplication> LeaseApplications { get; set; }
    }
}

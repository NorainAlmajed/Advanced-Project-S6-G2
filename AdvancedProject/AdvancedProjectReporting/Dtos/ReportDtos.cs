namespace AdvancedProjectReporting.Dtos
{
    public class DashboardViewModel
    {
        public List<OccupancyReportDto> Occupancy { get; set; } = new();
        public MaintenanceReportDto? Maintenance { get; set; }
        public PaymentReportDto? Payments { get; set; }
    }

    // Occupancy Report
    public class OccupancyReportDto
    {
        public string BuildingName { get; set; } = null!;
        public int TotalUnits { get; set; }
        public int OccupiedUnits { get; set; }
        public int VacantUnits { get; set; }
        public double OccupancyRate { get; set; }
    }

    // Maintenance Report
    public class MaintenanceReportDto
    {
        public int TotalRequests { get; set; }
        public int PendingRequests { get; set; }
        public int InProgressRequests { get; set; }
        public int ResolvedRequests { get; set; }
        public int ClosedRequests { get; set; }
        public double AverageResolutionDays { get; set; }
        public double AverageResolutionHours { get; set; }
        public List<MaintenanceByTypeDto> ByType { get; set; } = new();
        public List<ResolvedRequestDetailDto> ResolvedRequestDetails { get; set; } = new();
    }

    public class MaintenanceByTypeDto
    {
        public string Type { get; set; } = null!;
        public int Count { get; set; }
    }

    public class ResolvedRequestDetailDto
    {
        public int RequestId { get; set; }
        public string SkillType { get; set; } = null!;
        public DateTime RequestDate { get; set; }
        public DateTime ResolvedDate { get; set; }
        public double DaysToResolve { get; set; }
        public double HoursToResolve { get; set; }
        public string TimeToResolve { get; set; } = "00:00:00";
    }

    // Payment Report
    public class PaymentReportDto
    {
        public decimal TotalCollected { get; set; }
        public decimal TotalOutstanding { get; set; }
        public int OverdueCount { get; set; }
        public double IslamicProfitRate { get; set; }
        public List<OverduePaymentDto> OverduePayments { get; set; } = new();
    }

    public class OverduePaymentDto
    {
        public string TenantName { get; set; } = null!;
        public string UnitNumber { get; set; } = null!;
        public string PropertyName { get; set; } = null!;
        public decimal AmountDue { get; set; }
        public DateTime DueDate { get; set; }
        public int DaysOverdue { get; set; }
        public decimal ProfitAmount { get; set; }
        public decimal TotalAmountDue { get; set; }
    }
}

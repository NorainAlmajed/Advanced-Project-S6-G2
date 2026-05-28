namespace AdvancedProject.ViewModels
{
    public class PaginationVM
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string Action { get; set; } = "Index";
        public string Controller { get; set; } = "";
        public Dictionary<string, object?> RouteValues { get; set; } = new();
    }
}

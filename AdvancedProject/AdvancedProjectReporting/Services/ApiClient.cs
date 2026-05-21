using System.Net.Http.Headers;
using System.Net.Http.Json;
using AdvancedProjectReporting.Dtos;

namespace AdvancedProjectReporting.Services;

public class ApiClient
{
    private readonly HttpClient _http;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ApiClient(HttpClient http, IHttpContextAccessor httpContextAccessor)
    {
        _http = http;
        _httpContextAccessor = httpContextAccessor;
    }

    private void SetAuthHeader()
    {
        var token = _httpContextAccessor.HttpContext?.Session.GetString("JwtToken");
        if (!string.IsNullOrEmpty(token))
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
    }

    public async Task<LoginResponse?> LoginAsync(string email, string password)
    {
        var response = await _http.PostAsJsonAsync(
            "api/auth/login",
            new LoginRequest { Email = email, Password = password });

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<LoginResponse>();
    }

    public async Task<List<OccupancyReportDto>> GetOccupancyReportAsync()
    {
        SetAuthHeader();
        var result = await _http.GetFromJsonAsync<List<OccupancyReportDto>>(
            "api/reports/occupancy");
        return result ?? new List<OccupancyReportDto>();
    }

    public async Task<MaintenanceReportDto?> GetMaintenanceReportAsync()
    {
        SetAuthHeader();
        return await _http.GetFromJsonAsync<MaintenanceReportDto>(
            "api/reports/maintenance");
    }

    public async Task<PaymentReportDto?> GetPaymentReportAsync()
    {
        SetAuthHeader();
        return await _http.GetFromJsonAsync<PaymentReportDto>(
            "api/reports/payments");
    }
}

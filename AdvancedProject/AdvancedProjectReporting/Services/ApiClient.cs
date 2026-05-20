using System.Net.Http.Headers;
using System.Net.Http.Json;
using AdvancedProjectReporting.Dtos;


namespace AdvancedProjectReporting.Services;

public class ApiClient
{
    private readonly HttpClient _http;
    private readonly IConfiguration _config;
    private string? _token;
    private DateTime _tokenExpiry;

    public ApiClient(HttpClient http, IConfiguration config)
    {
        _http = http;
        _config = config;
    }

    // Automatically login and get token
    private async Task EnsureAuthenticatedAsync()
    {
        if (_token != null && DateTime.UtcNow < _tokenExpiry)
            return;

        var loginRequest = new LoginRequest
        {
            Email = _config["ApiSettings:ManagerEmail"]!,
            Password = _config["ApiSettings:ManagerPassword"]!
        };

        var response = await _http.PostAsJsonAsync(
            "api/auth/login", loginRequest);
        response.EnsureSuccessStatusCode();

        var loginResponse = await response.Content
            .ReadFromJsonAsync<LoginResponse>();

        _token = loginResponse!.Token;
        _tokenExpiry = loginResponse.ExpiresAt.AddMinutes(-1);

        // Set token for all future requests
        _http.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _token);
    }

    // Get Occupancy Report
    public async Task<List<OccupancyReportDto>>
        GetOccupancyReportAsync()
    {
        await EnsureAuthenticatedAsync();
        var result = await _http
            .GetFromJsonAsync<List<OccupancyReportDto>>(
                "api/reports/occupancy");
        return result ?? new List<OccupancyReportDto>();
    }

    // Get Maintenance Report
    public async Task<MaintenanceReportDto?>
        GetMaintenanceReportAsync()
    {
        await EnsureAuthenticatedAsync();
        return await _http
            .GetFromJsonAsync<MaintenanceReportDto>(
                "api/reports/maintenance");
    }

    // Get Payment Report
    public async Task<PaymentReportDto?>
        GetPaymentReportAsync()
    {
        await EnsureAuthenticatedAsync();
        return await _http
            .GetFromJsonAsync<PaymentReportDto>(
                "api/reports/payments");
    }
}
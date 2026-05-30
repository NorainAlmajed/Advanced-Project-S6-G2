using System.Net;
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

    // Shared helper — checks status code before deserializing
    private static async Task<T?> ReadAsync<T>(HttpResponseMessage response)
    {
        if (response.StatusCode == HttpStatusCode.Unauthorized)
            throw new SessionExpiredException();

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<T>();
    }

    // Returns null on bad credentials; throws ApiUnavailableException if API is down
    public async Task<LoginResponse?> LoginAsync(string email, string password)
    {
        try
        {
            var response = await _http.PostAsJsonAsync(
                "api/auth/login",
                new LoginRequest { Email = email, Password = password });

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<LoginResponse>();
        }
        catch (HttpRequestException ex)
        {
            throw new ApiUnavailableException(ex);
        }
    }

    public async Task<List<OccupancyReportDto>> GetOccupancyReportAsync()
    {
        SetAuthHeader();
        try
        {
            var response = await _http.GetAsync("api/reports/occupancy");
            var result   = await ReadAsync<List<OccupancyReportDto>>(response);
            return result ?? new List<OccupancyReportDto>();
        }
        catch (SessionExpiredException) { throw; }
        catch (Exception ex) when (ex is not SessionExpiredException) { throw new ApiUnavailableException(ex); }
    }

    public async Task<MaintenanceReportDto?> GetMaintenanceReportAsync()
    {
        SetAuthHeader();
        try
        {
            var response = await _http.GetAsync("api/reports/maintenance");
            return await ReadAsync<MaintenanceReportDto>(response);
        }
        catch (SessionExpiredException) { throw; }
        catch (Exception ex) when (ex is not SessionExpiredException) { throw new ApiUnavailableException(ex); }
    }

    public async Task<PaymentReportDto?> GetPaymentReportAsync()
    {
        SetAuthHeader();
        try
        {
            var response = await _http.GetAsync("api/reports/payments");
            return await ReadAsync<PaymentReportDto>(response);
        }
        catch (SessionExpiredException) { throw; }
        catch (Exception ex) when (ex is not SessionExpiredException) { throw new ApiUnavailableException(ex); }
    }
}

public class SessionExpiredException : Exception
{
    public SessionExpiredException()
        : base("Your session has expired. Please sign in again.") { }
}

public class ApiUnavailableException : Exception
{
    public ApiUnavailableException(Exception inner)
        : base("The API server is unavailable. Please ensure the API is running.", inner) { }
}

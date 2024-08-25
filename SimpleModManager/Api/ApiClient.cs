using Newtonsoft.Json;
using Serilog;
using SimpleModManager.Util;

namespace SimpleModManager.Api;

public sealed class ApiClient : IDisposable
{
    private static int _remainingDailyRequest = 100;
    private static int _remainingHourlyRequest = 100;
    private readonly HttpClient _client;

    private readonly ILogger _logger;

    public ApiClient()
    {
        _logger = LoggerHandler.GetLogger<ApiClient>();
        _client = new HttpClient();
    }

    public void Dispose()
    {
        _logger.Debug("Disposing...");
        _client.Dispose();
    }

    public HttpResponseMessage Send(HttpRequestMessage request)
    {
        EnsureNexusRateLimit(request);
        _logger.Information("Sending request: {0} {1} {2}", request.Method, request.Version, request.RequestUri);
        var response = _client.Send(request);
        UpdateNexusRateLimitValues(response);
        return response;
    }

    public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
    {
        EnsureNexusRateLimit(request);
        _logger.Information("Sending request as async: {0} {1} {2}", request.Method, request.Version,
            request.RequestUri);
        var response = await _client.SendAsync(request);
        UpdateNexusRateLimitValues(response);
        return response;
    }

    private static void EnsureNexusRateLimit(HttpRequestMessage request)
    {
        var isNexus = IsRequestToNexus(request);
        switch (isNexus)
        {
            case true when _remainingHourlyRequest <= 0:
                throw new HttpRequestException("Hourly rate-limited. Wait till next hour, to continue.");
            case true when _remainingDailyRequest <= 0:
                throw new HttpRequestException("Daily rate-limited. Wait till tomorrow, to continue.");
        }
    }

    private static bool IsRequestToNexus(HttpRequestMessage request)
    {
        var isNexus = request.RequestUri != null &&
                      request.RequestUri.Host.Contains("nexusmods", StringComparison.CurrentCultureIgnoreCase);
        return isNexus;
    }

    public async Task<ModInfoApi> RequestModInfo(string gameName, int modId)
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            Version = Version.Parse("2.0"),
            RequestUri = new Uri($"https://api.nexusmods.com/v1/games/{gameName}/mods/{modId}.json"),
            Headers =
            {
                { "accept", "application/json" },
                { "apikey", $"{ModManager.Apikey}" }
            }
        };
        using (var response = await SendAsync(request))
        {
            response.EnsureSuccessStatusCode();

            var body = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<ModInfoApi>(body) ??
                   throw new Exception("Couldn't deserialize response from NexusMods");
        }
    }

    private void UpdateNexusRateLimitValues(HttpResponseMessage response)
    {
        if (response.RequestMessage != null && !IsRequestToNexus(response.RequestMessage)) return;
        var foundHourly = response.Headers.TryGetValues("x-rl-hourly-remaining", out var hourlyRemainingValues);
        var foundDaily = response.Headers.TryGetValues("x-rl-daily-remaining", out var dailyRemainingValues);

        if (!foundHourly || hourlyRemainingValues == null)
            throw new Exception("Couldn't find hourly rate limit in headers");
        if (!foundDaily || dailyRemainingValues == null)
            throw new Exception("Couldn't find Daily rate limit in headers");
        if (hourlyRemainingValues.Any(hourlyRemainingValue =>
                int.TryParse(hourlyRemainingValue, out _remainingHourlyRequest)))
            _logger.Debug("Updated hourly nexus rate limit to {0}.", _remainingHourlyRequest);

        if (dailyRemainingValues.Any(dailyRemainingValue =>
                int.TryParse(dailyRemainingValue, out _remainingDailyRequest)))
            _logger.Debug("Updated daily nexus rate limit to {0}.", _remainingDailyRequest);
    }
}
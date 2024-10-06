using IdentityModel.Client;

public class ApiClientWorker : BackgroundService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger _logger;

    public ApiClientWorker(IHttpClientFactory httpClientFactory, ILogger<ApiClientWorker> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var token = await GetTokenFromIdentityServerAsync();

            if (!string.IsNullOrEmpty(token))
            {
                await CallProtectedApiAsync(token);
            }

            // Delay for some time (e.g., 5 minutes) before making the next API call
            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }
    }

    private async Task<string> GetTokenFromIdentityServerAsync()
    {
        // Use HttpClient to call IdentityServer and retrieve the access token
        using var client = _httpClientFactory.CreateClient();
        var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
        {
            Address = "http://localhost:5000/connect/token",
            ClientId = "client",
            ClientSecret = "secret",
            Scope = "api1"
        });

        return tokenResponse.AccessToken;
    }

    private async Task CallProtectedApiAsync(string token)
    {
        // Use the token to call the protected API
        using var client = _httpClientFactory.CreateClient();
        client.SetBearerToken(token);

        var response = await client.GetAsync("http://localhost:5010/weatherforecast");
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            _logger.LogInformation($"API response: {content}");
        }
        else
        {
            _logger.LogError($"Failed to access the protected API");
        }
    }
}

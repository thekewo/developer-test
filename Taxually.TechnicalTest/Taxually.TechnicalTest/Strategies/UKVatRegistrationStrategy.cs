public class UKVatRegistrationStrategy : IVatRegistrationStrategy
{
    private readonly ITaxuallyHttpClient _httpClient;
    private readonly ILogger<UKVatRegistrationStrategy> _logger;

    public UKVatRegistrationStrategy(
        ITaxuallyHttpClient httpClient)
    {
        _httpClient = httpClient;
        _logger = new Logger<UKVatRegistrationStrategy>(new LoggerFactory());
    }

    public async Task RegisterAsync(VatRegistrationRequest request)
    {
        _logger.LogInformation("Registering {CompanyId} in the UK via API", request.CompanyId);
        await _httpClient.PostAsync("https://api.uktax.gov.uk", request);
        _logger.LogInformation("Successfully registered {CompanyId} in the UK", request.CompanyId);
    }
}
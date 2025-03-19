using Taxually.TechnicalTest.Exceptions;

public class VatRegistrationStrategyFactory : IVatRegistrationStrategyFactory
{
    private readonly ITaxuallyHttpClient _httpClient;
    private readonly ITaxuallyQueueClient _queueClient;

    public VatRegistrationStrategyFactory(ITaxuallyHttpClient httpClient, ITaxuallyQueueClient queueClient)
    {
        _httpClient = httpClient;
        _queueClient = queueClient;
    }

    public IVatRegistrationStrategy GetStrategy(string countryCode)
    {
        switch (countryCode)
        {
            case "GB":
                return new UKVatRegistrationStrategy(_httpClient);
            case "FR":
                return new FranceVatRegistrationStrategy(_queueClient);
            case "DE":
                return new GermanyVatRegistrationStrategy(_queueClient);
            default:
                throw new UnsupportedCountryException(countryCode);
        }
    }
}
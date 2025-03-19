public interface IVatRegistrationStrategyFactory
{
    IVatRegistrationStrategy GetStrategy(string countryCode);
}
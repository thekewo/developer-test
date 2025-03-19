namespace Taxually.TechnicalTest.Exceptions;

public class UnsupportedCountryException : Exception
{
    public UnsupportedCountryException(string countryCode)
        : base($"Country not supported: {countryCode}")
    {
    }
}

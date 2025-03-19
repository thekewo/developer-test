public interface IVatRegistrationStrategy
{
    Task RegisterAsync(VatRegistrationRequest request);
}
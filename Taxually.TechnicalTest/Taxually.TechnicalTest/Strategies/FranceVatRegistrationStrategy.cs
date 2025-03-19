using System.Text;

public class FranceVatRegistrationStrategy : IVatRegistrationStrategy
{
    private readonly ITaxuallyQueueClient _queueClient;

    public FranceVatRegistrationStrategy(ITaxuallyQueueClient queueClient)
    {
        _queueClient = queueClient;
    }

    public async Task RegisterAsync(VatRegistrationRequest request)
    {
        var csvBuilder = new StringBuilder();
        csvBuilder.AppendLine("CompanyName,CompanyId");
        csvBuilder.AppendLine($"{request.CompanyName},{request.CompanyId}");
        var csv = Encoding.UTF8.GetBytes(csvBuilder.ToString());
        await _queueClient.EnqueueAsync("vat-registration-csv", csv);
    }
}
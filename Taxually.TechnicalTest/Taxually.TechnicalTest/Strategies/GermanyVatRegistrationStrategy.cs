using System.Xml.Serialization;

public class GermanyVatRegistrationStrategy : IVatRegistrationStrategy
{
    private readonly ITaxuallyQueueClient _queueClient;

    public GermanyVatRegistrationStrategy(ITaxuallyQueueClient queueClient)
    {
        _queueClient = queueClient;
    }

    public async Task RegisterAsync(VatRegistrationRequest request)
    {
        using (var stringWriter = new StringWriter())
        {
            var serializer = new XmlSerializer(typeof(VatRegistrationRequest));
            serializer.Serialize(stringWriter, this);
            var xml = stringWriter.ToString();
            await _queueClient.EnqueueAsync("vat-registration-xml", xml);
        }
    }
}
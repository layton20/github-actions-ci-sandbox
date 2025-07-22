using Azure.Messaging.ServiceBus;

namespace BookWrom.Services;

public class MessageSenderService
{
    private const string SERVICE_BUS_CONNECTION_STRING = "ServiceBus:ConnectionString";
    private readonly ServiceBusSender __Sender;

    public MessageSenderService(IConfiguration configuration)
    {
        string __ConnectionString = configuration[SERVICE_BUS_CONNECTION_STRING] ?? string.Empty;
        ServiceBusClient _Client = new(__ConnectionString);
        __Sender = _Client.CreateSender("orders");
    }

    public async Task SendMessageAsync(string messageContent)
    {
        ServiceBusMessage message = new(messageContent);
        await __Sender.SendMessageAsync(message);
    }
}
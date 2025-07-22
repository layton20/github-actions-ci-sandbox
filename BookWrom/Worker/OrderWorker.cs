using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Worker;

public class OrderWorker : BackgroundService
{
    private const string SERVICE_BUS_CONNECTION_STRING = "ServiceBus:ConnectionString";
    private const string SERVICE_BUS_QUEUE_NAME = "orders";
    private readonly IConfiguration __Configuration;
    private readonly ILogger<OrderWorker> __Logger;
    private ServiceBusProcessor __Processor;

    public OrderWorker(IConfiguration configuration, ILogger<OrderWorker> logger)
    {
        __Configuration = configuration;
        __Logger = logger;
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        string _ConnectionString = __Configuration[SERVICE_BUS_CONNECTION_STRING] ?? string.Empty;
        ServiceBusClient _Client = new(_ConnectionString);

        __Processor = _Client.CreateProcessor(SERVICE_BUS_QUEUE_NAME, new ServiceBusProcessorOptions());
        __Processor.ProcessMessageAsync += async args =>
        {
            string body = args.Message.Body.ToString();
            __Logger.LogInformation($"Received message: {body}");

            await Task.Delay(500, cancellationToken);

            await args.CompleteMessageAsync(args.Message, cancellationToken);
        };

        __Processor.ProcessErrorAsync += args =>
        {
            __Logger.LogError(args.Exception, "Error receiving message");
            return Task.CompletedTask;
        };

        await __Processor.StartProcessingAsync(cancellationToken);

        await base.StartAsync(cancellationToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await __Processor.StopProcessingAsync(cancellationToken);
        await __Processor.DisposeAsync();

        await base.StopAsync(cancellationToken);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.CompletedTask;
    }
}
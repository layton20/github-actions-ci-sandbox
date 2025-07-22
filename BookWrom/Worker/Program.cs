using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Worker;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        IConfigurationRoot builtConfig = config.Build();

        // Add Azure Key Vault
        config.AddAzureKeyVault(
            new Uri("https://azure-sandbox-lej1-kv.vault.azure.net/"),
            new DefaultAzureCredential());
    })
    .ConfigureServices((context, services) => { services.AddHostedService<OrderWorker>(); })
    .Build();

await host.RunAsync();
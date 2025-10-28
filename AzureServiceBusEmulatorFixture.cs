using Azure.Messaging.ServiceBus;
using DotNet.Testcontainers.Builders;
using Testcontainers.ServiceBus;
using Xunit;

namespace TesttFixtures.Utilities;

public class AzureServiceBusEmulatorFixture : IAsyncLifetime
{
    private readonly ServiceBusContainer _serviceBusContainer;
    public const ushort ServiceBusPort = 5672;
    public const ushort ServiceBusHttpPort = 5300;

    public AzureServiceBusEmulatorFixture()
    {
        _serviceBusContainer = new ServiceBusBuilder()
            .WithImage("mcr.microsoft.com/azure-messaging/servicebus-emulator:latest")
            .WithAcceptLicenseAgreement(true)
            .WithPortBinding(ServiceBusPort, true)
            .WithPortBinding(ServiceBusHttpPort, true)
            .WithEnvironment("SQL_WAIT_INTERVAL", "0")
            .WithResourceMapping("Config.json", "/ServiceBus_Emulator/ConfigFiles/")
            .WithWaitStrategy(Wait.ForUnixContainer().UntilHttpRequestIsSucceeded(request =>
                request.ForPort(ServiceBusHttpPort).ForPath("/health")))
            .Build();
    }

    public async Task InitializeAsync()
    {
        await _serviceBusContainer.StartAsync();
    }

    public string GetConnectionString()
    {
        var hostPort = _serviceBusContainer.GetMappedPublicPort(ServiceBusPort);
        return $"Endpoint=sb://localhost:{hostPort}/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey==aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa=;UseDevelopmentEmulator=true;";
    }

    public async Task DisposeAsync()
    {
        await _serviceBusContainer.DisposeAsync();
    }
}

using WireMock.Server;
using WireMock.Settings;

namespace TestFixtures.Utilities;

public class WireMockServerFixture : IDisposable
{
    private readonly WireMockServer wireMockServer;

    public WireMockServerFixture()
    {
        wireMockServer = WireMockServer.Start(/*port: 5980*/ new WireMockServerSettings
        {
            UseSSL = false,
            Port = 5980,
            AcceptAnyClientCertificate = true,
            StartAdminInterface = true
        });
    }

    public WireMockServer GetWireMockServer()
    {
        return wireMockServer;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected void Dispose(bool disposing)
    {
        if (disposing)
        {
            wireMockServer.Dispose();
        }
    }
}

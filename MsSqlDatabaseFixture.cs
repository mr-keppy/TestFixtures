using DotNet.Testcontainers.Builders;
using Testcontainers.MsSql;
using Xunit;

namespace TestFixtures.Utilities;

public class MsSqlDatabaseFixture : IAsyncLifetime
{
    public const string Database = "master";
    public const string Username = "sa";
    public const string Password = "5tR0n6-P455w0rd-Th4t-M3375-5Ql-R3qU1r3m3n75!";
    public const ushort MsSqlPort = 1433;
    public MsSqlContainer DbContainer { get; } = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-CU18-ubuntu-22.04")
        .WithPortBinding(MsSqlPort, true)
        .WithEnvironment("ACCEPT_EULA", "Y")
        .WithEnvironment("SQLCMDUSER", Username)
        .WithEnvironment("SQLCMDPASSWORD", Password)
        .WithEnvironment("MSSQL_SA_PASSWORD", Password)
        .WithWaitStrategy(Wait
            .ForUnixContainer()
            .UntilCommandIsCompleted(
                "/opt/mssql-tools18/bin/sqlcmd",
                "-C",
                "-Q",
                "SELECT 1;"))
        .Build();

    public async Task InitializeAsync()
    {
        await await DbContainer.StartAsync();
    }

    public Task DisposeAsync()
    {
        return DbContainer.DisposeAsync().AsTask();
    }
}

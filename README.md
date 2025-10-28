# 🧪 Integration Test Fixtures for .NET

This repository provides a set of **reusable C# xUnit test fixtures** to simplify integration testing for applications that depend on external services like **Azure Service Bus**, **Azure Blob Storage**, **SQL Server**, and **HTTP APIs**.

Each fixture is designed to **emulate or isolate dependencies** for consistent, reliable, and repeatable integration testing — without requiring live cloud infrastructure.

---

## 🚀 Overview

| Fixture | Purpose |
|----------|----------|
| **`AzureBlobStorageEmulatorFixture`** | Spins up and manages an Azure Blob Storage emulator instance for testing blob upload/download operations. |
| **`AzureServiceBusEmulatorFixture`** | Starts a local Azure Service Bus emulator to publish and receive messages without connecting to the real Service Bus. |
| **`MsSqlDatabaseFixture`** | Creates and manages a clean SQL Server test database for integration tests, with automatic setup and teardown between runs. |
| **`WireMockServerFixture`** | Hosts a lightweight HTTP mock server using WireMock.Net to simulate external API calls and verify request/response behavior. |

---

## 🧩 Usage

All fixtures are implemented as **xUnit [Collection Fixtures](https://xunit.net/docs/shared-context#collection-fixture)**, so they can be easily shared across tests.

### Example 1: Using `MsSqlDatabaseFixture`

```csharp
[Collection("Database collection")]
public class SampleRepositoryTests
{
    private readonly MsSqlDatabaseFixture _db;

    public SampleRepositoryTests(MsSqlDatabaseFixture db)
    {
        _db = db;
    }

    [Fact]
    public async Task Should_Insert_And_Retrieve_Sample()
    {
        var context = _db.CreateDbContext();
        // Arrange
        var sample = new Sample { Name = "Test Policy" };
        context.Samples.Add(policy);
        await context.SaveChangesAsync();

        // Act
        var retrieved = await context.Samples.FindAsync(sample.Id);

        // Assert
        Assert.Equal("Test Sample", retrieved.Name);
    }
}
```
### Example 2 — Using AzureBlobStorageEmulatorFixture (Class Fixture)
```csharp
public class BlobStorageTests : IClassFixture<AzureBlobStorageEmulatorFixture>
{
    private readonly AzureBlobStorageEmulatorFixture _blobFixture;

    public BlobStorageTests(AzureBlobStorageEmulatorFixture blobFixture)
    {
        _blobFixture = blobFixture;
    }

    [Fact]
    public async Task Should_Upload_And_Download_Blob_Successfully()
    {
        // Arrange
        var containerClient = _blobFixture.GetBlobContainerClient("test-container");
        await containerClient.CreateIfNotExistsAsync();

        var blobClient = containerClient.GetBlobClient("sample.txt");
        var content = new BinaryData("Hello from integration tests!");

        // Act
        await blobClient.UploadAsync(new MemoryStream(content.ToArray()), overwrite: true);
        var downloaded = await blobClient.DownloadContentAsync();

        // Assert
        Assert.Equal("Hello from integration tests!", downloaded.Value.Content.ToString());
    }
}
```

### Prerequisites

.NET 8.0+

xUnit (for test execution)

Docker/Podman (for emulators like SQL Server, Service Bus, Blob Storage)

WireMock.Net (for mock HTTP servers)

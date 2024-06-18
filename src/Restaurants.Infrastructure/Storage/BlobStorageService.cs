using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.Extensions.Options;
using Restaurants.Domain.Interfaces;
using Restaurants.Infrastructure.Configuration;

namespace Restaurants.Infrastructure.Storage;

public class BlobStorageService(IOptions<BlobStorageSettings> blobStorageSettingsOptions) : IBlobStorageService
{
    private readonly BlobStorageSettings _blobStorageSettings = blobStorageSettingsOptions.Value;
    public async Task<string> UploadToBlobAsync(Stream data, string fileName)
    {
        BlobServiceClient blobServiceClient = new BlobServiceClient(_blobStorageSettings.ConnectionString);
        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(_blobStorageSettings.LogosContainerName);
        BlobClient blobClient = containerClient.GetBlobClient(fileName);
        await blobClient.UploadAsync(data);
        string blobUri = blobClient.Uri.ToString();
        return blobUri;
    }

    public string? GetBlobSasUrl(string? blobUrl)
    {
        if (blobUrl is null)
        {
            return null;
        }

        var sasBulilder = new BlobSasBuilder()
        {
            BlobContainerName = _blobStorageSettings.LogosContainerName,
            Resource = "b",
            StartsOn = DateTimeOffset.UtcNow,
            ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(30),
            BlobName = GetBlobNameFromUrl(blobUrl)
        };
        sasBulilder.SetPermissions(BlobAccountSasPermissions.Read);

        BlobServiceClient blobServiceClient = new BlobServiceClient(_blobStorageSettings.ConnectionString);

        var sasToken = sasBulilder.ToSasQueryParameters(new Azure.Storage.StorageSharedKeyCredential(blobServiceClient.AccountName, _blobStorageSettings.AccountKey)).ToString();
        return $"{blobUrl}?{sasToken}";
    }
    private string GetBlobNameFromUrl(string blobUrl)
    {
        var uri = new Uri(blobUrl);
        return uri.Segments.Last();
    }
}
using Azure.Storage.Blobs;
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
}
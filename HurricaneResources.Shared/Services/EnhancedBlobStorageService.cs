using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;
using HurricaneResources.Shared.Configuration;

namespace HurricaneResources.Shared.Services;

/// <summary>
/// Enhanced Azure Blob Storage service that supports both connection strings and managed identity
/// </summary>
public class EnhancedBlobStorageService : IBlobStorageService
{
    private readonly AzureBlobStorageOptions _options;
    private readonly BlobServiceClient _blobServiceClient;

    public EnhancedBlobStorageService(IOptions<AzureBlobStorageOptions> options)
    {
        _options = options.Value ?? throw new ArgumentNullException(nameof(options));
        
        if (!_options.IsValid())
        {
            throw new InvalidOperationException("Azure Blob Storage configuration is invalid. Please check your appsettings.");
        }

        // Try managed identity first, fall back to connection string
        if (!string.IsNullOrEmpty(_options.ConnectionString) && 
            !_options.ConnectionString.Equals("YOUR_AZURE_STORAGE_CONNECTION_STRING_HERE", StringComparison.OrdinalIgnoreCase))
        {
            _blobServiceClient = new BlobServiceClient(_options.ConnectionString);
        }
        else if (!string.IsNullOrEmpty(_options.BaseUrl))
        {
            // Use managed identity for Azure production deployment
            var credential = new DefaultAzureCredential();
            _blobServiceClient = new BlobServiceClient(new Uri(_options.BaseUrl), credential);
        }
        else
        {
            throw new InvalidOperationException("Neither connection string nor base URL with managed identity is configured.");
        }
    }

    public async Task<string> UploadIconAsync(Stream stream, string fileName, string contentType = "image/jpeg")
    {
        try
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_options.ContainerName);
            
            // Ensure container exists
            await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

            var blobClient = containerClient.GetBlobClient(fileName);

            var blobHttpHeaders = new BlobHttpHeaders
            {
                ContentType = contentType
            };

            await blobClient.UploadAsync(stream, new BlobUploadOptions
            {
                HttpHeaders = blobHttpHeaders
            });

            return _options.GetIconUrl(fileName);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to upload icon: {ex.Message}", ex);
        }
    }

    public async Task<bool> DeleteIconAsync(string fileName)
    {
        try
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_options.ContainerName);
            var blobClient = containerClient.GetBlobClient(fileName);
            
            var response = await blobClient.DeleteIfExistsAsync();
            return response.Value;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to delete icon: {ex.Message}", ex);
        }
    }

    public async Task<bool> IconExistsAsync(string fileName)
    {
        try
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_options.ContainerName);
            var blobClient = containerClient.GetBlobClient(fileName);
            
            var response = await blobClient.ExistsAsync();
            return response.Value;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to check if icon exists: {ex.Message}", ex);
        }
    }

    public async Task<Stream?> GetIconStreamAsync(string fileName)
    {
        try
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_options.ContainerName);
            var blobClient = containerClient.GetBlobClient(fileName);

            if (await blobClient.ExistsAsync())
            {
                var response = await blobClient.DownloadStreamingAsync();
                return response.Value.Content;
            }

            return null;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to get icon stream: {ex.Message}", ex);
        }
    }

    public async Task<List<string>> ListIconsAsync()
    {
        try
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_options.ContainerName);
            
            if (!await containerClient.ExistsAsync())
            {
                return new List<string>();
            }

            var icons = new List<string>();
            await foreach (var blobItem in containerClient.GetBlobsAsync())
            {
                icons.Add(blobItem.Name);
            }

            return icons;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to list icons: {ex.Message}", ex);
        }
    }

    public string GetIconUrl(string fileName)
    {
        return _options.GetIconUrl(fileName);
    }

    public string GetDefaultIconUrl()
    {
        return _options.DefaultIconUrl;
    }
}
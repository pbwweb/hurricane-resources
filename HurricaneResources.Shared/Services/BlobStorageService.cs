using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;
using HurricaneResources.Shared.Configuration;

namespace HurricaneResources.Shared.Services;

/// <summary>
/// Azure Blob Storage service implementation
/// </summary>
public class BlobStorageService : IBlobStorageService
{
    private readonly AzureBlobStorageOptions _options;
    private readonly BlobServiceClient _blobServiceClient;

    public BlobStorageService(IOptions<AzureBlobStorageOptions> options)
    {
        _options = options.Value ?? throw new ArgumentNullException(nameof(options));
        
        if (!_options.IsValid())
        {
            throw new InvalidOperationException("Azure Blob Storage configuration is invalid. Please check your appsettings.");
        }

        _blobServiceClient = new BlobServiceClient(_options.ConnectionString);
    }

    public async Task<string> UploadIconAsync(Stream stream, string fileName, string contentType = "image/jpeg")
    {
        try
        {
            var containerClient = await GetContainerClientAsync();
            
            // Get the blob client for the file
            var blobClient = containerClient.GetBlobClient(fileName);
            
            // Upload the file with metadata
            var blobUploadOptions = new BlobUploadOptions
            {
                HttpHeaders = new BlobHttpHeaders
                {
                    ContentType = contentType
                },
                Metadata = new Dictionary<string, string>
                {
                    ["UploadedAt"] = DateTime.UtcNow.ToString("O"),
                    ["Category"] = fileName.Split('-')[0] // Extract category from filename
                }
            };

            await blobClient.UploadAsync(stream, blobUploadOptions);
            
            // Return the URL of the uploaded file
            return _options.GetIconUrl(fileName);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to upload icon '{fileName}': {ex.Message}", ex);
        }
    }

    public async Task<bool> DeleteIconAsync(string fileName)
    {
        try
        {
            var containerClient = await GetContainerClientAsync();
            var blobClient = containerClient.GetBlobClient(fileName);
            
            var response = await blobClient.DeleteIfExistsAsync();
            return response.Value;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> IconExistsAsync(string fileName)
    {
        try
        {
            var containerClient = await GetContainerClientAsync();
            var blobClient = containerClient.GetBlobClient(fileName);
            
            var response = await blobClient.ExistsAsync();
            return response.Value;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public string GenerateIconFileName(string category, string originalFileName)
    {
        var extension = Path.GetExtension(originalFileName).ToLowerInvariant();
        var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        var cleanCategory = category.ToLowerInvariant().Replace(" ", "-");
        
        return $"{cleanCategory}-{timestamp}{extension}";
    }

    private async Task<BlobContainerClient> GetContainerClientAsync()
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_options.ContainerName);
        await containerClient.CreateIfNotExistsAsync(Azure.Storage.Blobs.Models.PublicAccessType.Blob);
        return containerClient;
    }

    private static string GetContentType(string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        return extension switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".webp" => "image/webp",
            _ => "application/octet-stream"
        };
    }
}
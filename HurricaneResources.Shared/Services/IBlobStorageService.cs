

namespace HurricaneResources.Shared.Services;

/// <summary>
/// Service for managing Azure Blob Storage operations for hurricane resource icons
/// </summary>
public interface IBlobStorageService
{
    /// <summary>
    /// Uploads an icon file to Azure Blob Storage
    /// </summary>
    /// <param name="stream">The file stream to upload</param>
    /// <param name="fileName">The desired filename in blob storage</param>
    /// <param name="contentType">The content type of the file</param>
    /// <returns>The URL of the uploaded file</returns>
    Task<string> UploadIconAsync(Stream stream, string fileName, string contentType = "image/jpeg");

    /// <summary>
    /// Deletes an icon file from Azure Blob Storage
    /// </summary>
    /// <param name="fileName">The filename to delete</param>
    /// <returns>True if deleted successfully, false if file didn't exist</returns>
    Task<bool> DeleteIconAsync(string fileName);

    /// <summary>
    /// Checks if an icon file exists in Azure Blob Storage
    /// </summary>
    /// <param name="fileName">The filename to check</param>
    /// <returns>True if the file exists</returns>
    Task<bool> IconExistsAsync(string fileName);

    /// <summary>
    /// Generates a unique filename for an icon based on category and original filename
    /// </summary>
    /// <param name="category">The resource category</param>
    /// <param name="originalFileName">The original filename</param>
    /// <returns>A unique filename for blob storage</returns>
    static string GenerateIconFileName(string category, string originalFileName)
    {
        var fileExtension = Path.GetExtension(originalFileName).ToLowerInvariant();
        var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        var categoryName = category.ToLowerInvariant().Replace(" ", "-");
        return $"{categoryName}-{timestamp}{fileExtension}";
    }
}
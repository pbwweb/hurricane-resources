namespace HurricaneResources.Shared.Configuration;

public class AzureBlobStorageOptions
{
    public const string SectionName = "AzureBlobStorage";

    /// <summary>
    /// Azure Storage Account connection string
    /// </summary>
    public string ConnectionString { get; set; } = string.Empty;

    /// <summary>
    /// Container name for storing icons
    /// </summary>
    public string ContainerName { get; set; } = "icons";

    /// <summary>
    /// Base URL for the storage account (e.g., https://youraccount.blob.core.windows.net)
    /// </summary>
    public string BaseUrl { get; set; } = string.Empty;

    /// <summary>
    /// Default icon URL to use as fallback when no specific icon is available
    /// </summary>
    public string DefaultIconUrl { get; set; } = "https://pbwblobs.blob.core.windows.net/icons/emergency-20250916003531.jpg";

    /// <summary>
    /// Gets the full URL for an icon file
    /// </summary>
    /// <param name="fileName">The icon filename</param>
    /// <returns>Full URL to the icon</returns>
    public string GetIconUrl(string fileName)
    {
        return $"{BaseUrl.TrimEnd('/')}/{ContainerName}/{fileName}";
    }

    /// <summary>
    /// Validates that all required settings are configured
    /// </summary>
    /// <returns>True if configuration is valid</returns>
    public bool IsValid()
    {
        return !string.IsNullOrWhiteSpace(ConnectionString) &&
               !string.IsNullOrWhiteSpace(ContainerName) &&
               !string.IsNullOrWhiteSpace(BaseUrl);
    }
}
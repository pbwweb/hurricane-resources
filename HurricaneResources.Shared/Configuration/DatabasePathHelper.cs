namespace HurricaneResources.Shared.Configuration;

/// <summary>
/// Helper class for managing database path configuration across all applications
/// </summary>
public static class DatabasePathHelper
{
    /// <summary>
    /// Gets the standardized database path for the Hurricane Resources application
    /// </summary>
    /// <returns>Absolute path to the shared database file</returns>
    public static string GetDatabasePath()
    {
        // Get the solution root directory (two levels up from any project)
        var projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", ".."));
        
        // Ensure the assets directory exists
        var assetsDir = Path.Combine(projectRoot, "assets");
        Directory.CreateDirectory(assetsDir);
        
        // Return the full path to the database
        return Path.Combine(assetsDir, "hurricane_resources.db");
    }

    /// <summary>
    /// Gets the connection string for the shared database
    /// </summary>
    /// <returns>SQLite connection string</returns>
    public static string GetConnectionString()
    {
        var dbPath = GetDatabasePath();
        return $"Data Source={dbPath}";
    }

    /// <summary>
    /// Gets the assets directory path
    /// </summary>
    /// <returns>Absolute path to the assets directory</returns>
    public static string GetAssetsDirectory()
    {
        var projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", ".."));
        return Path.Combine(projectRoot, "assets");
    }
}
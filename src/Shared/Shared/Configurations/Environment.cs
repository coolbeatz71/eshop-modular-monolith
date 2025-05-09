namespace EShop.Shared;

/// <summary>
/// Provides access to environment-specific configuration values used in the application.
/// </summary>
public static class Environment
{
    /// <summary>
    /// Retrieves postgresql database configuration values from environment variables.
    /// </summary>
    /// <remarks>
    /// This method is intended for development environments where connection details
    /// are provided via environment variables (e.g., from a .env file or local shell).
    ///
    /// Expected environment variables:
    /// - POSTGRES_PORT
    /// - POSTGRES_DB
    /// - POSTGRES_USER
    /// - POSTGRES_PASSWORD
    /// </remarks>
    /// <returns>
    /// A tuple containing:
    /// - <c>port</c>: The postgresql port (e.g., "5432")
    /// - <c>db</c>: The database name
    /// - <c>user</c>: The database username
    /// - <c>pass</c>: The database password
    /// </returns>
    public static (string? port, string? db, string? user, string? pass) Database()
    {
        var port = System.Environment.GetEnvironmentVariable("POSTGRES_PORT");
        var db   = System.Environment.GetEnvironmentVariable("POSTGRES_DB");
        var user = System.Environment.GetEnvironmentVariable("POSTGRES_USER");
        var pass = System.Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");

        return (port, db, user, pass);
    }
}

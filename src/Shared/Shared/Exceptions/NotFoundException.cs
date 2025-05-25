namespace Eshop.Shared.Exceptions;

/// <summary>
/// Exception that represents a not found error, typically indicating that a requested resource does not exist.
/// </summary>
public class NotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NotFoundException"/> class with a custom message.
    /// </summary>
    /// <param name="message">The error message that describes the missing resource.</param>
    public NotFoundException(string message) : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="NotFoundException"/> class with an entity name and a key.
    /// Automatically removes the word "Entity" from the entity name if present.
    /// </summary>
    /// <param name="entityName">The name of the entity type.</param>
    /// <param name="key">The unique identifier of the entity that was not found.</param>
    public NotFoundException(string entityName, object key)
        : base($"Could not find {CleanEntityName(entityName)} with id: {key}")
    {
    }

    /// <summary>
    /// Removes the word "Entity" (case-insensitive) from the entity name, if present.
    /// </summary>
    /// <param name="name">The original entity name.</param>
    /// <returns>The cleaned entity name.</returns>
    private static string CleanEntityName(string name)
    {
        return name.Replace("entity", "", StringComparison.OrdinalIgnoreCase).Trim();
    }
}
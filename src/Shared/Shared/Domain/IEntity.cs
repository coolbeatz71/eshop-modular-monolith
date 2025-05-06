namespace EShop.Shared.Domain;

/// <summary>
/// A base generic interface for entities with a typed identifier.
/// </summary>
/// <typeparam name="T">The type of the entity's identifier.</typeparam>
public interface IEntity<T> : IEntity
{
    /// <summary>
    /// Gets or sets the identifier of the entity.
    /// </summary>
    public T Id { get; set; }
}

/// <summary>
/// A non-generic base interface for entities.
/// </summary>
public interface IEntity
{
    /// <summary>
    /// Gets or sets the creation timestamp of the entity.
    /// </summary>
    public DateTime? CreatedAt { get; set; }
    
    /// <summary>
    /// Gets or sets the identifier of the creator of the entity.
    /// </summary>
    public string? CreatedBy { get; set; }
    
    /// <summary>
    /// Gets or sets the last updated timestamp of the entity.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
    
    /// <summary>
    /// Gets or sets the identifier of the last updater of the entity.
    /// </summary>
    public string ? UpdatedBy { get; set; }
}
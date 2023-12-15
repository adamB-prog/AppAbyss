namespace AppAbyss.Repository.Repositories.Interfaces;

/// <summary>
/// Generic repository interface for entities
/// </summary>
/// <typeparam name="T">The type of the entity</typeparam>
public interface IGenericRepository<T> where T : class
{
    /// <summary>
    /// Asynchronously adds the given entity to the repository.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task AddAsync(T entity);

    /// <summary>
    /// Asynchronously deletes the given entity from the repository.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task DeleteAsync(T entity);

    /// <summary>
    /// Asynchronously retrieves all entities from the repository.
    /// </summary>
    /// <returns>An <see cref="IEnumerable{T}"/> containing all entities in the repository.</returns>
    Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    /// Asynchronously retrieves the entity with the given ID from the repository.
    /// </summary>
    /// <param name="id">The ID of the entity to retrieve.</param>
    /// <returns>The entity with the given ID, or null if no such entity exists.</returns>
    Task<T?> GetByIdAsync(object id);

    /// <summary>
    /// Asynchronously updates the given entity in the repository.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task UpdateAsync(T entity);
}
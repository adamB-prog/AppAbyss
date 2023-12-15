using AppAbyss.Data;
using AppAbyss.Repository.Repositories.Base;
using AppAbyss.Repository.Repositories.Interfaces;

namespace AppAbyss.Repository.Repositories.Entities;

/// <summary>
/// Repository class for OperatingSystemInfo entities.
/// </summary>
public class OperatingSystemRepository:GenericRepository<OperatingSystemInfo>,IOperatingSystemRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OperatingSystemRepository"/> class.
    /// </summary>
    /// <param name="context">The <see cref="ApplicationDbContext"/> to use for database operations.</param>
    public OperatingSystemRepository(ApplicationDbContext context) : base(context)
    {
    }
}
using AppAbyss.Data;
using AppAbyss.Repository.Repositories.Base;
using AppAbyss.Repository.Repositories.Interfaces;

namespace AppAbyss.Repository.Repositories.Entities;

/// <summary>
/// Repository class for Software entities.
/// </summary>
public class SoftwareRepository:GenericRepository<Software>,ISoftwareRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SoftwareRepository"/> class.
    /// </summary>
    /// <param name="context">The <see cref="ApplicationDbContext"/> to use for database operations.</param>
    public SoftwareRepository(ApplicationDbContext context) : base(context)
    {
    }
}
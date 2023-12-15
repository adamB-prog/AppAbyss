using AppAbyss.Data;
using AppAbyss.Repository.Repositories.Base;
using AppAbyss.Repository.Repositories.Interfaces;

namespace AppAbyss.Repository.Repositories.Entities;

/// <summary>
/// Repository class for SoftwareList entities.
/// </summary>
public class SoftwareListRepository:GenericRepository<SoftwareList>,ISoftwareListRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SoftwareListRepository"/> class.
    /// </summary>
    /// <param name="context">The <see cref="ApplicationDbContext"/> to use for database operations.</param>
    public SoftwareListRepository(ApplicationDbContext context) : base(context)
    {
    }
}
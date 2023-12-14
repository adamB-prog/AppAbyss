using AppAbyss.Data;
using AppAbyss.Repository.Repositories.Base;
using AppAbyss.Repository.Repositories.Interfaces;

namespace AppAbyss.Repository.Repositories.Entities;

/// <summary>
/// Repository class for AppUser entities.
/// </summary>
public class AppUserRepository:GenericRepository<AppUser>,IAppUserRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AppUserRepository"/> class.
    /// </summary>
    /// <param name="context">The <see cref="ApplicationDbContext"/> to use for database operations.</param>
    public AppUserRepository(ApplicationDbContext context) : base(context)
    {
    }
}
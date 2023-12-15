using AppAbyss.Data;
using AppAbyss.Repository.Repositories.Base;
using AppAbyss.Repository.Repositories.Interfaces;

namespace AppAbyss.Repository.Repositories.Entities;

/// <summary>
/// Repository class for Icon entities.
/// </summary>
public class IconRepository: GenericRepository<Icon>,IIconRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IconRepository"/> class.
    /// </summary>
    /// <param name="context">The <see cref="ApplicationDbContext"/> to use for database operations.</param>
    public IconRepository(ApplicationDbContext context) : base(context)
    {
    }
}
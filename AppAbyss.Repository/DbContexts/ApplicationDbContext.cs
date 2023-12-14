using AppAbyss.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AppAbyss.Repository;

/// <summary>
/// Represents the application's database context.
/// </summary>
public class ApplicationDbContext : IdentityDbContext
{
    
    /// <summary>
    /// Gets or sets the collection of AppUsers in the database.
    /// </summary>
    public new DbSet<AppUser> Users { get; set; }
    
    /// <summary>
    /// Gets or sets the collection of Icons in the database.
    /// </summary>
    public new DbSet<Icon> Icons { get; set; }
    
    /// <summary>
    /// Gets or sets the collection of OperatingSystemInfo in the database.
    /// </summary>
    public new DbSet<OperatingSystemInfo> OperatingSystemInfos { get; set; }
    
    /// <summary>
    /// Gets or sets the collection of Software in the database.
    /// </summary>
    public new DbSet<Software> Software { get; set; }
    
    /// <summary>
    /// Gets or sets the collection of SoftwareList in the database.
    /// </summary>
    public new DbSet<SoftwareList> SoftwareLists { get; set; }
    
    
    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
    /// </summary>
    /// <param name="options"></param>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        
        builder.Entity<Icon>()
            .HasMany(t => t.SoftwareItems)
            .WithOne(s=>s.Icon)
            .HasForeignKey(s => s.SoftwareId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<OperatingSystemInfo>()
            .HasMany(t => t.SoftwareItems)
            .WithOne(s=>s.OperatingSystemInfo)
            .HasForeignKey(s => s.OperatingSystemId)
            .OnDelete(DeleteBehavior.Cascade);


        builder.Entity<Software>()
            .HasMany(s => s.SoftwareLists)
            .WithMany(c => c.SoftwareItems)
            .UsingEntity(j => j.ToTable("SoftwareListElement"));


        builder.Entity<AppUser>()
            .HasMany(s => s.UserSoftwareLists)
            .WithOne(c => c.AppUser)
            .HasForeignKey(x => x.AppUserId);

        builder.Entity<SoftwareList>()
            .HasOne(x => x.AppUser)
            .WithOne(z => z.FaveriteSoftwareList)
            .HasForeignKey<AppUser>(o => o.FavoriteSoftwareListId);
        
        

        base.OnModelCreating(builder);
    }
}
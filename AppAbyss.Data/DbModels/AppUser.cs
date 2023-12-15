using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace AppAbyss.Data;

/// <summary>
/// Represents a user.
/// </summary>
public class AppUser: IdentityUser
{
    
    /// <summary>
    /// Gets or sets the collection of <see cref="SoftwareList"/> items for this user.
    /// </summary>
    public virtual ICollection<SoftwareList> UserSoftwareLists { get; set; }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="AppUser"/> class.
    /// </summary>
    public AppUser()
    {
        this.UserSoftwareLists = new HashSet<SoftwareList>();
    }
}
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace AppAbyss.Data;

/// <summary>
/// Represents a user.
/// </summary>
public class AppUser: IdentityUser
{
    
    /// <summary>
    /// Gets or sets the ID of the favorite software list.
    /// </summary>
    public int FavoriteSoftwareListId { get; set; }
    
    /// <summary>
    /// Gets or sets the favorite software list.
    /// </summary>
    [ForeignKey("FavoriteSoftwareListId")]
    public virtual SoftwareList FaveriteSoftwareList { get; set; }
    
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
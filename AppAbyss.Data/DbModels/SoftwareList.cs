using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppAbyss.Data;

/// <summary>
/// Represents a software list.
/// </summary>
public class SoftwareList
{
    /// <summary>
    ///  Gets or sets the ID of the software list.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int SoftwareListId { get; set; }
    
    /// <summary>
    /// Gets or sets the name of the software list.
    /// </summary>
    [Required]
    public string Name { get; set; }
    
    /// <summary>
    /// Gets or sets the collection of <see cref="Software"/> items in this software list.
    /// </summary>
    public virtual ICollection<Software> SoftwareItems { get; set; }
    
    /// <summary>
    /// Gets or sets the ID of the user that owns this software list.
    /// </summary>
    [Required]
    public int AppUserId { get; set; }
    
    /// <summary>
    /// Gets or sets the user that owns this software list.
    /// </summary>
    [ForeignKey("AppUserId")]
    public virtual AppUser AppUser { get; set; }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="SoftwareList"/> class.
    /// </summary>
    public SoftwareList()
    {
        this.SoftwareItems = new HashSet<Software>();
    }
}
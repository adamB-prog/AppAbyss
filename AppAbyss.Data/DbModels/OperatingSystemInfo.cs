using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppAbyss.Data;

/// <summary>
/// Represents an operating system.
/// </summary>
public class OperatingSystemInfo
{
    /// <summary>
    /// Gets or sets the ID of the operating system.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IconId { get; set; }
    
    /// <summary>
    /// Gets or sets the name of the operating system.
    /// </summary>
    [Required]
    string Name { get; set; }
    
    /// <summary>
    /// Gets or sets the collection of <see cref="Software"/> items that use this operating system.
    /// </summary>
    public ICollection<Software> SoftwareItems { get; set; }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="OperatingSystemInfo"/> class.
    /// </summary>
    public OperatingSystemInfo()
    {
        this.SoftwareItems = new HashSet<Software>();
    }
}
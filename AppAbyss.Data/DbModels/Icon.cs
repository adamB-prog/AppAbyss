using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppAbyss.Data;

/// <summary>
/// Represents an icon.
/// </summary>
public class Icon
{
    /// <summary>
    /// Gets or sets the ID of the icon.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IconId { get; set; }
    
    /// <summary>
    /// Gets or sets the url of the icon.
    /// </summary>
    [Required]
    public required string Url { get; set; }
    
    /// <summary>
    /// Gets or sets the alternative url of the icon.
    /// </summary>
    public required string AlternativeUrl { get; set; }
    
    /// <summary>
    /// Gets or sets the collection of <see cref="Software"/> items that use this icon.
    /// </summary>
    public ICollection<Software> SoftwareItems { get; set; }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="Icon"/> class.
    /// </summary>
    public Icon()
    {
        this.SoftwareItems = new HashSet<Software>();
    }
}
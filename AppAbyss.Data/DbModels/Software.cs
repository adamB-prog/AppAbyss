using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppAbyss.Data;

/// <summary>
/// Represents a software.
/// </summary>
public class Software
{
    /// <summary>
    /// Gets or sets the ID of the software.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int SoftwareId { get; set; }
    
    /// <summary>
    /// Gets or sets the name of the software.
    /// </summary>
    [Required]
    public string Name { get; set; }
    
    /// <summary>
    /// Gets or sets the full description of the software.
    /// </summary>
    [Required]
    public string FullDescription { get; set; }
    
    /// <summary>
    /// Gets or sets the short description of the software.
    /// </summary>
    [Required]
    public string ShortDescription { get; set; }
    
    /// <summary>
    /// Gets or sets the version of the software.
    /// </summary>
    [Required]
    public string Version { get; set; }
    
    /// <summary>
    /// Gets or sets the source url of the software.
    /// </summary>
    [Required]
    public string SourceURL { get; set; }
    
    /// <summary>
    /// Gets or sets the release date of the software.
    /// </summary>
    public DateTime ReleaseDate { get; set; }
    
    /// <summary>
    /// Gets or sets the icon ID of the software.
    /// </summary>
    /// <remarks>
    /// This is a foreign key reference to the Icon table.
    /// </remarks>
    public int IconId { get; set; }
    
    /// <summary>
    /// Gets or sets the icon of the software.
    /// </summary>
    [ForeignKey("IconId")]
    public virtual Icon Icon { get; set; }
    
    /// <summary>
    /// Gets or sets the operating system ID of the software.
    /// </summary>
    /// <remarks>
    /// This is a foreign key reference to the OperatingSystemInfo table.
    /// </remarks>
    [Required]
    public int OperatingSystemId { get; set; }
    
    /// <summary>
    /// Gets or sets the operating system of the software.
    /// </summary>
    [ForeignKey("OperatingSystemId")]
    public virtual OperatingSystemInfo OperatingSystemInfo { get; set; }
    
    /// <summary>
    /// Gets or sets the collection of <see cref="SoftwareList"/> items in this software list.
    /// </summary>
    public virtual ICollection<SoftwareList> SoftwareLists { get; set; }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="Software"/> class.
    /// </summary>
    public Software()
    {
        this.SoftwareLists = new HashSet<SoftwareList>();
    }
}
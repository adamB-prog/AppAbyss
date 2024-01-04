using System.ComponentModel.DataAnnotations;

namespace AppAbyss.Data.Dtos.Auth;

public class LoginDto
{
    /// <summary>
    /// Gets or sets the user's username.
    /// </summary>
    [Required(ErrorMessage = "Username is required")]
    [StringLength(256, MinimumLength = 3)]
    public string UserName { get; set; }

    /// <summary>
    /// Gets or sets the user's password.
    /// </summary>
    [StringLength(256, MinimumLength = 8)]
    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}
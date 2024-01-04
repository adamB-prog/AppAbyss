using System.ComponentModel.DataAnnotations;

namespace AppAbyss.Data.Dtos.Auth;

/// <summary>
/// Represents a model for user registration.
/// </summary>
public class RegisterDto
{

    /// <summary>
    /// Gets or sets the user's email address.
    /// </summary>
    [Required(ErrorMessage = "Email is required")]
    [DataType(DataType.EmailAddress)]
    [EmailAddress]
    public string Email { get; set; }

    /// <summary>
    /// Gets or sets the user's username.
    /// </summary>
    [StringLength(256, MinimumLength = 3)]
    [Required(ErrorMessage = "Username is required")]
    public string Username { get; set; }

    /// <summary>
    /// Gets or sets the user's password.
    /// </summary>
    [StringLength(256, MinimumLength = 8)]
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }
}
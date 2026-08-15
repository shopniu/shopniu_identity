
using System.ComponentModel.DataAnnotations;
namespace Shopniu_identity.Aplication.Authentication.Models;

public class LoginViewModel
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; } = default!;
    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = default!;
    public bool RememberMe { get; set; }
    public string? ReturnUrl { get; set; }
}
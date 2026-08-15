
using System.ComponentModel.DataAnnotations;
namespace Shopniu_identity.Aplication.Authentication.Models;

public class RegisterViewModel
{
    [Required(ErrorMessage = "El nombre es requerido.")]
    [StringLength(50, ErrorMessage = "El nombre no puede superar los 50 caracteres.")]
    public string FirstName { get; set; } = default!;

    [Required(ErrorMessage = "El apellido es requerido.")]
    [StringLength(50, ErrorMessage = "El apellido no puede superar los 50 caracteres.")]
    public string LastName { get; set; } = default!;

    [Required(ErrorMessage = "El email es requerido.")]
    [EmailAddress(ErrorMessage = "Ingresá un email válido.")]
    public string Email { get; set; } = default!;

    public string? UserName { get; set; }

    [Required(ErrorMessage = "La contraseña es requerida.")]
    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$",
        ErrorMessage = "La contraseña debe contener mayúsculas, minúsculas y números.")]
    public string Password { get; set; } = default!;

    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Las contraseñas no coinciden.")]
    public string ConfirmPassword { get; set; } = default!;

    public string? ReturnUrl { get; set; }
}

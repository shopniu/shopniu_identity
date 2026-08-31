namespace Shopniu_identity.Application.Users.UseCases.RegisterUser;

/// <summary>
/// Alta de cuenta sin contraseña del cliente: el backend genera la contraseña.
/// Lo usa el BFF al registrar al invitado desde el checkout.
/// </summary>
public record AutoRegisterUserCommand(string FirstName, string LastName, string Email);

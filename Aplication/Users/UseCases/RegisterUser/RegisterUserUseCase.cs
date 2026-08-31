
using Microsoft.AspNetCore.Identity;
using Shopniu_identity.Domain.Entities.UserEntity;

namespace Shopniu_identity.Application.Users.UseCases.RegisterUser;

public class RegisterUserUseCase
{
    private readonly UserManager<User> _userManager;

    public RegisterUserUseCase(UserManager<User> userManager) => _userManager = userManager;

    public async Task<RegisterUserResult> ExecuteAsync(RegisterUserCommand command)
    {
        var userName = command.UserName?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(userName))
            userName = await GenerateUserNameAsync(command.Email);

        var user = new User(command.FirstName, command.LastName, command.Email, userName);

        var result = await _userManager.CreateAsync(user, command.Password);
        if (!result.Succeeded)
            return RegisterUserResult.Failure(result.Errors.Select(e => e.Description));

        return RegisterUserResult.Success(user);
    }

    /// <summary>
    /// Alta sin credenciales del cliente: el backend genera la contraseña y la
    /// devuelve (solo al BFF) para poder emitir tokens sin exponerla al
    /// navegador. El envío por correo es una iteración futura.
    /// </summary>
    public async Task<RegisterUserAutoResult> ExecuteAutoAsync(string firstName, string lastName, string email)
    {
        var userName = await GenerateUserNameAsync(email);
        var password = PasswordGenerator.Generate();

        var user = new User(firstName, lastName, email, userName);

        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
            return RegisterUserAutoResult.Failure(result.Errors.Select(e => e.Description));

        return RegisterUserAutoResult.Success(user, password);
    }

    private async Task<string> GenerateUserNameAsync(string email)
    {
        var baseName = email.Split('@')[0].Trim();

        var candidate = baseName;
        var suffix = 1;

        while (await _userManager.FindByNameAsync(candidate) is not null)
        {
            candidate = $"{baseName}{suffix++}";
        }

        return candidate;
    }
}

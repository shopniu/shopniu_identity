
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

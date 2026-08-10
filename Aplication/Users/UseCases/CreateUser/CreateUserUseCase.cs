
using Microsoft.AspNetCore.Identity;
using Shopniu_identity.Domain.Entities.UserEntity;
using Shopniu_identity.Domain.Exceptions.Common;


namespace Shopniu_identity.Application.Users.UseCases.CreateUser;

public class CreateUserUseCase
{
    private readonly UserManager<User> _userManager;

    public CreateUserUseCase(UserManager<User> userManager) => _userManager = userManager;

    public async Task<User> ExecuteAsync(CreateUserCommand command)
    {
        var user = new User(command.FirstName, command.LastName, command.Email, command.UserName);

        var result = await _userManager.CreateAsync(user, command.Password);
        if (!result.Succeeded)
            throw new ValidationsException(string.Join(", ", result.Errors.Select(e => e.Description)));

        return user;
    }
}
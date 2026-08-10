using Microsoft.AspNetCore.Identity;
using Shopniu_identity.Domain.Entities.UserEntity;
using Microsoft.EntityFrameworkCore;


namespace Shopniu_identity.Application.Users.UseCases.GetAllUsers;

public class GetAllUsersUseCase
{
    private readonly UserManager<User> _userManager;

    public GetAllUsersUseCase(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<List<User>> ExecuteAsync()
    {
        var users = await _userManager.Users.ToListAsync();
        return users;
    }

}
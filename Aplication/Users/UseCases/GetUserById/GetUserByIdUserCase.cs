
using Shopniu_identity.Domain.Exceptions.Common;
using Shopniu_identity.Domain.Entities.UserEntity;
using Microsoft.AspNetCore.Identity;


namespace Shopniu_identity.Application.Users.UseCases.GetUserById;

public class GetUserByIdUserCase
{
    private readonly UserManager<User> _userRepository;

    public GetUserByIdUserCase(UserManager<User> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User> Execute(int userId)
    {
        var user = await _userRepository.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            throw new NotFoundException("User", userId);
        }
        return user;
    }
}

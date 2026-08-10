
using Shopniu_identity.Application.Users.UseCases.GetUserById;
using Shopniu_identity.Domain.Entities.UserEntity;
using Shopniu_identity.Application.Users.UseCases.GetAllUsers;
using Shopniu_identity.Application.Users.UseCases.CreateUser;

namespace Shopniu_identity.Application.Users;

public class UserHandler
{
    private readonly GetUserByIdUserCase _getUserByIdUserCase;
    private readonly GetAllUsersUseCase _getAllUsersUseCase;
    private readonly CreateUserUseCase _createUserUseCase;

    public UserHandler(GetUserByIdUserCase getUserByIdUserCase, GetAllUsersUseCase getAllUsersUseCase, CreateUserUseCase createUserUseCase)
    {
        _getUserByIdUserCase = getUserByIdUserCase;
        _getAllUsersUseCase = getAllUsersUseCase;
        _createUserUseCase = createUserUseCase;
    }

    public async Task<User> HandleGetUserById(int userId)
    {
        return await _getUserByIdUserCase.Execute(userId);
    }

    public async Task<List<User>> HandleGetAllUsers()
    {
        return await _getAllUsersUseCase.ExecuteAsync();
    }

    public async Task<User> HandleCreateUser(CreateUserCommand command)
    {
        return await _createUserUseCase.ExecuteAsync(command);
    }


}
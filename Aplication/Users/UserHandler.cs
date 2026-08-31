
using Shopniu_identity.Application.Users.UseCases.GetUserById;
using Shopniu_identity.Domain.Entities.UserEntity;
using Shopniu_identity.Application.Users.UseCases.GetAllUsers;
using Shopniu_identity.Application.Users.UseCases.RegisterUser;

namespace Shopniu_identity.Application.Users;

public class UserHandler
{
    private readonly GetUserByIdUserCase _getUserByIdUserCase;
    private readonly GetAllUsersUseCase _getAllUsersUseCase;
    private readonly RegisterUserUseCase _registerUserUseCase;

    public UserHandler(GetUserByIdUserCase getUserByIdUserCase, GetAllUsersUseCase getAllUsersUseCase, RegisterUserUseCase registerUserUseCase)
    {
        _getUserByIdUserCase = getUserByIdUserCase;
        _getAllUsersUseCase = getAllUsersUseCase;
        _registerUserUseCase = registerUserUseCase;
    }

    public async Task<User> HandleGetUserById(int userId)
    {
        return await _getUserByIdUserCase.Execute(userId);
    }

    public async Task<List<User>> HandleGetAllUsers()
    {
        return await _getAllUsersUseCase.ExecuteAsync();
    }

    public async Task<RegisterUserResult> HandleRegisterUser(RegisterUserCommand command)
    {
        return await _registerUserUseCase.ExecuteAsync(command);
    }

    public async Task<RegisterUserAutoResult> HandleRegisterAutoUser(AutoRegisterUserCommand command)
    {
        return await _registerUserUseCase.ExecuteAutoAsync(command.FirstName, command.LastName, command.Email);
    }


}
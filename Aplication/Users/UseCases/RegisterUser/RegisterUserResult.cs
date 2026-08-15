
using Shopniu_identity.Domain.Entities.UserEntity;

namespace Shopniu_identity.Application.Users.UseCases.RegisterUser;

public class RegisterUserResult
{
    public User? User { get; }
    public bool Succeeded => User is not null;
    public IEnumerable<string> Errors { get; }

    private RegisterUserResult(User? user, IEnumerable<string> errors)
    {
        User = user;
        Errors = errors;
    }

    public static RegisterUserResult Success(User user) => new(user, Array.Empty<string>());

    public static RegisterUserResult Failure(IEnumerable<string> errors) => new(null, errors);
}

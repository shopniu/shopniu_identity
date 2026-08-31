
using Shopniu_identity.Domain.Entities.UserEntity;

namespace Shopniu_identity.Application.Users.UseCases.RegisterUser;

public class RegisterUserAutoResult
{
    public User? User { get; }
    public string? GeneratedPassword { get; }
    public bool Succeeded => User is not null;
    public IEnumerable<string> Errors { get; }

    private RegisterUserAutoResult(User? user, string? generatedPassword, IEnumerable<string> errors)
    {
        User = user;
        GeneratedPassword = generatedPassword;
        Errors = errors;
    }

    public static RegisterUserAutoResult Success(User user, string generatedPassword)
        => new(user, generatedPassword, Array.Empty<string>());

    public static RegisterUserAutoResult Failure(IEnumerable<string> errors)
        => new(null, null, errors);
}

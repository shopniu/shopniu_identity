
using System.Security.Cryptography;

namespace Shopniu_identity.Application.Users.UseCases.RegisterUser;

/// <summary>
/// Genera contraseñas seguras en el backend (nunca en el navegador). Cumple la
/// policy de Identity: mínimo 8 caracteres con mayúsculas, minúsculas y
/// dígitos. El valor en claro solo existe en el servidor durante el alta y se
/// descarta al persistir el hash; el envío por correo es una iteración futura.
/// </summary>
public static class PasswordGenerator
{
    private const string Upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string Lower = "abcdefghijklmnopqrstuvwxyz";
    private const string Digits = "0123456789";
    private const string All = Upper + Lower + Digits;

    public static string Generate(int length = 16)
    {
        if (length < 8)
        {
            throw new ArgumentOutOfRangeException(nameof(length), "La contraseña debe tener al menos 8 caracteres.");
        }

        var chars = new char[length];

        chars[0] = Pick(Upper);
        chars[1] = Pick(Lower);
        chars[2] = Pick(Digits);

        for (var i = 3; i < length; i++)
        {
            chars[i] = Pick(All);
        }

        Shuffle(chars);

        return new string(chars);
    }

    private static char Pick(string charset)
    {
        return charset[RandomNumberGenerator.GetInt32(charset.Length)];
    }

    private static void Shuffle(char[] chars)
    {
        for (var i = chars.Length - 1; i > 0; i--)
        {
            var j = RandomNumberGenerator.GetInt32(i + 1);
            (chars[i], chars[j]) = (chars[j], chars[i]);
        }
    }
}

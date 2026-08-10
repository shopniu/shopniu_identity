using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shopniu_identity.Domain.Entities.UserEntity;

namespace Shopniu_identity.Routes;

public class AccountController : Controller
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;

    public AccountController(SignInManager<User> signInManager, UserManager<User> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [HttpGet("~/account/login")]
    public IActionResult Login(string? returnUrl = null)
    {
        var html = $$"""
            <html>
            <body style="font-family: sans-serif; max-width: 320px; margin: 80px auto;">
                <h3>Shopniu — Iniciar sesión</h3>
                <form method="post" action="/account/login?returnUrl={{Uri.EscapeDataString(returnUrl ?? "/")}}">
                    <input name="email" type="email" placeholder="Email" required style="width:100%;padding:8px;margin-bottom:8px;" />
                    <input name="password" type="password" placeholder="Password" required style="width:100%;padding:8px;margin-bottom:8px;" />
                    <button type="submit" style="width:100%;padding:8px;">Ingresar</button>
                </form>
            </body>
            </html>
            """;

        return Content(html, "text/html");
    }

    [HttpPost("~/account/login")]
    public async Task<IActionResult> Login(string email, string password, string? returnUrl = null)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            return Content("Credenciales inválidas.", "text/plain");

        var result = await _signInManager.PasswordSignInAsync(user, password, isPersistent: false, lockoutOnFailure: true);

        if (result.IsLockedOut)
            return Content("Tu cuenta está bloqueada temporalmente. Intentá más tarde.", "text/plain");

        if (!result.Succeeded)
            return Content("Credenciales inválidas.", "text/plain");

        return LocalRedirect(returnUrl ?? "/");
    }
}
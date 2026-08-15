using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using Shopniu_identity.Aplication.Authentication.Models;
using Shopniu_identity.Application.Users.UseCases.RegisterUser;
using Shopniu_identity.Domain.Entities.UserEntity;

namespace Shopniu_identity.Routes;

public class AccountController : Controller
{
    private const string WebClientId = "shopniu-web";

    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly RegisterUserUseCase _registerUserUseCase;
    private readonly IOpenIddictApplicationManager _applicationManager;

    public AccountController(SignInManager<User> signInManager, UserManager<User> userManager, RegisterUserUseCase registerUserUseCase, IOpenIddictApplicationManager applicationManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _registerUserUseCase = registerUserUseCase;
        _applicationManager = applicationManager;
    }

    [HttpGet("~/account/login")]
    public IActionResult Login(string? returnUrl = null)
    {
        return View(new LoginViewModel { ReturnUrl = returnUrl });
    }

    [HttpPost("~/account/login")]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "Credenciales inválidas.");
            return View(model);
        }

        var result = await _signInManager.PasswordSignInAsync(user, model.Password, isPersistent: model.RememberMe, lockoutOnFailure: true);

        if (result.IsLockedOut)
        {
            ModelState.AddModelError(string.Empty, "Tu cuenta está bloqueada temporalmente. Intentá más tarde.");
            return View(model);
        }

        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, "Credenciales inválidas.");
            return View(model);
        }

        if (!string.IsNullOrWhiteSpace(model.ReturnUrl))
            return LocalRedirect(model.ReturnUrl);

        ViewData["Title"] = "Sesión iniciada — Shopniu";
        ViewData["Message"] = "Tu sesión se inició correctamente.";
        ViewData["ActionUrl"] = await GetFrontOriginAsync();
        ViewData["ActionText"] = "Continuar";
        return View("Success");
    }

    [HttpGet("~/account/register")]
    public IActionResult Register(string? returnUrl = null)
    {
        return View(new RegisterViewModel { ReturnUrl = returnUrl });
    }

    [HttpPost("~/account/register")]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var result = await _registerUserUseCase.ExecuteAsync(new RegisterUserCommand(
            model.FirstName,
            model.LastName,
            model.Email,
            model.UserName,
            model.Password));

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error);

            return View(model);
        }

        if (!string.IsNullOrWhiteSpace(model.ReturnUrl))
        {
            await _signInManager.SignInAsync(result.User!, isPersistent: false);
            return LocalRedirect(model.ReturnUrl);
        }

        ViewData["Title"] = "Cuenta creada — Shopniu";
        ViewData["Message"] = "Tu cuenta fue creada correctamente.";
        ViewData["ActionUrl"] = Url.Action(nameof(Login), "Account");
        ViewData["ActionText"] = "Iniciar sesión";
        return View("Success");
    }

    private async Task<string?> GetFrontOriginAsync()
    {
        var application = await _applicationManager.FindByClientIdAsync(WebClientId);
        if (application is null)
            return null;

        var redirectUris = await _applicationManager.GetRedirectUrisAsync(application);
        var firstUri = redirectUris.FirstOrDefault();

        return firstUri is null ? null : new Uri(firstUri).GetLeftPart(UriPartial.Authority);
    }
}

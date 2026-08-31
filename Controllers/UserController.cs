

using Microsoft.AspNetCore.Mvc;
using Shopniu_identity.Application.Users;
using Shopniu_identity.Application.Users.UseCases.RegisterUser;

namespace Shopniu_identity.Controllers;

[Route("api/v1/users")]
public class UserController : Controller
{
    private readonly UserHandler _userHandler;

    public UserController(UserHandler userHandler)
    {
        _userHandler = userHandler;
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserById(int userId)
    {
        var user = await _userHandler.HandleGetUserById(userId);
        return Ok(user);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userHandler.HandleGetAllUsers();
        return Ok(users);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] RegisterUserCommand command)
    {
        var result = await _userHandler.HandleRegisterUser(command);

        if (!result.Succeeded)
            return BadRequest(new { errors = result.Errors });

        return CreatedAtAction(nameof(GetUserById), new { userId = result.User!.Id }, result.User);
    }

    /// <summary>
    /// Alta de cuenta sin contraseña del cliente (backend la genera). Lo usa el
    /// BFF desde el checkout para registrar al invitado y emitir tokens vía
    /// password grant sin exponer credenciales al navegador. Devuelve la
    /// contraseña generada SOLO a este caller (servidor a servidor).
    /// </summary>
    [HttpPost("auto-register")]
    public async Task<IActionResult> AutoRegisterUser([FromBody] AutoRegisterUserCommand command)
    {
        var result = await _userHandler.HandleRegisterAutoUser(command);

        if (!result.Succeeded)
            return BadRequest(new { errors = result.Errors });

        return Ok(new
        {
            user = new
            {
                id = result.User!.Id,
                email = result.User.Email,
                firstName = result.User.FirstName,
                lastName = result.User.LastName
            },
            generatedPassword = result.GeneratedPassword
        });
    }


}
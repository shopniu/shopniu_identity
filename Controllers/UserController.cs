

using Microsoft.AspNetCore.Mvc;
using Shopniu_identity.Application.Users;

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
    public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command)
    {
        var user = await _userHandler.HandleCreateUser(command);
        return CreatedAtAction(nameof(GetUserById), new { userId = user.Id }, user);
    }


}
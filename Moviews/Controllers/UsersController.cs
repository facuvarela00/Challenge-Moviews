using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Moviews.Exceptions;
using Moviews.Models;
using Moviews.Services;

namespace MovieApi.Controllers;

[ApiController]
[Route("usuario")]
public class UsersController : ControllerBase
{
    private readonly IUserService _service;

    public UsersController(IUserService service)
    {
        _service = service;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO request)
    {
        try
        {
            var user = await _service.LoginAsync(request.Username, request.Password);

            return Ok(new
            {
                user.Id,
                user.UserName,
                user.IsAdmin
            });
        }
        catch (InvalidCredentialsException ex)
        {
            return Unauthorized(ex.Message);
        }
    }

}

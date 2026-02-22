using Microsoft.AspNetCore.Authorization;
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
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginDTO request)
    {

        try
        {
            var token = await _service.LoginAsync(request.Username, request.Password);

            return Ok(new
            {
                token = token
            });
        }
        catch (InvalidCredentialsException ex)
        {
            return Unauthorized(ex.Message);
        }
    }

}

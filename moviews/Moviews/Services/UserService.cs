using Microsoft.IdentityModel.Tokens;
using Moviews.Exceptions;
using Moviews.Models;
using Moviews.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;

namespace Moviews.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly IConfiguration _configuration;

    public UserService(IUserRepository repository, IConfiguration configuration)
    {
        _repository = repository;
        _configuration = configuration;
    }

    public async Task<string> LoginAsync(string username, string password)
    {
        var user = await _repository.GetByUsernameAsync(username);

        if (user == null || user.Password != password)
            throw new InvalidCredentialsException();

        return GenerateJwtToken(user);
    }

    public async Task<User> GetUserByIdAsync(Guid id)
    {

        var user = await _repository.GetByIdAsync(id);

        if (user == null)
            throw new UserNotFoundException(id);

        return user;
    }

    private string GenerateJwtToken(User user)
    {
        var claims = new[]
        {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim(ClaimTypes.Role, user.IsAdmin ? "Admin" : "User")
    };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)
        );

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddHours(2),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

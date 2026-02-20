using Moviews.Exceptions;
using Moviews.Models;
using Moviews.Repositories;
using System.Security.Authentication;

namespace Moviews.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;

    public UserService(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<User> LoginAsync(string username, string password)
    {
        var user = await _repository.GetByUsernameAsync(username);

        if (user == null || user.Password != password)
            throw new InvalidCredentialsException();

        return user;
    }

    public async Task<User> GetUserByIdAsync(Guid id)
    {

        var user = await _repository.GetByIdAsync(id);

        if (user == null)
            throw new UserNotFoundException(id);

        return user;
    }

}

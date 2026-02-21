using Moviews.Models;

namespace Moviews.Services;

public interface IUserService
{
    Task<string> LoginAsync(string username, string password);
    Task<User> GetUserByIdAsync(Guid id);
}

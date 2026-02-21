using Moviews.Models;

namespace Moviews.Repositories;

public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username);

    Task<User?> GetByIdAsync(Guid id);

}

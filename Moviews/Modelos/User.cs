using Moviews.Models;

public class User
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? PhotoUrl { get; set; }
    public bool IsAdmin { get; set; } = false;
    public bool Deleted { get; set; } = false;

}

public class UserDTO
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string? PhotoUrl { get; set; }
}

public class LoginDTO
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}


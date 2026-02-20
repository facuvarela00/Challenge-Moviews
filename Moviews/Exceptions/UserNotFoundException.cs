namespace Moviews.Exceptions;

public class UserNotFoundException : Exception
{
    public UserNotFoundException(Guid id)
        : base($"User con ID {id} no encontrado")
    {
    }
}

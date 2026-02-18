namespace MovieApi.Exceptions;

public class MovieNotFoundException : Exception
{
    public MovieNotFoundException()
        : base("Película no encontrada")
    {
    }

    public MovieNotFoundException(Guid id)
        : base($"Película con ID {id} no encontrada")
    {
    }
}

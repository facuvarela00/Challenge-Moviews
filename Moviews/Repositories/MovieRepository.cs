using MovieApi.Models;

namespace MovieApi.Repositories;

public class MovieRepository : IMovieRepository
{
    private readonly List<Movie> _movies = new();

    public IEnumerable<Movie> GetAll()
        => _movies;

    public Movie? GetById(Guid id)
        => _movies.FirstOrDefault(m => m.Id == id);

    public void Add(Movie movie)
        => _movies.Add(movie);

    public void Update(Movie movie)
    {
    }
}

using MovieApi.Models;

namespace MovieApi.Repositories;

public interface IMovieRepository
{
    IEnumerable<Movie> GetAll();
    Movie? GetById(Guid id);
    void Add(Movie movie);
    void Update(Movie movie);
}

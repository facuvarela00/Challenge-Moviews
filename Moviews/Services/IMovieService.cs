using MovieApi.Models;

namespace MovieApi.Services;

public interface IMovieService
{
    IEnumerable<Movie> GetAll();
    Movie? GetById(Guid id);
    Movie Create(Movie movie);
    Movie Update(Guid id, Movie updatedMovie);
    Movie Delete(Guid id);
    Review AddReview(Guid movieId, Review review);
}

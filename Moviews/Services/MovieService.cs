using MovieApi.Exceptions;
using MovieApi.Models;
using MovieApi.Repositories;

namespace MovieApi.Services;

public class MovieService : IMovieService
{
    private readonly IMovieRepository _repository;

    public MovieService(IMovieRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<Movie> GetAll()
        => _repository.GetAll().Where(m => !m.Deleted);

    public Movie GetById(Guid id)
    {
        var movie = _repository.GetById(id);

        if (movie == null || movie.Deleted)
            throw new MovieNotFoundException(id);

        return movie;
    }


    public Movie Create(Movie movie)
    {
        _repository.Add(movie);
        return movie;
    }

    public Movie Update(Guid id, Movie updatedMovie)
    {
        var movie = GetById(id);

        movie.Title = updatedMovie.Title;
        movie.Genre = updatedMovie.Genre;
        movie.ReleaseYear = updatedMovie.ReleaseYear;

        _repository.Update(movie);
        return movie;
    }


    public Movie Delete(Guid id)
    {
        var movie = GetById(id);

        movie.Deleted = true;

        _repository.Update(movie);

        return movie;
    }


    public Review AddReview(Guid movieId, Review review)
    {
        var movie = GetById(movieId);

        if (review.Rating < 1 || review.Rating > 5)
            throw new InvalidReviewException("El rating debe estar entre 1 y 5");

        if (string.IsNullOrWhiteSpace(review.Comment))
            throw new InvalidReviewException("El comentario es obligatorio");

        movie.Reviews.Add(review);

        _repository.Update(movie);

        return review;
    }


}

using Moviews.Exceptions;
using Moviews.Models;
using Moviews.Repositories;
using Microsoft.Extensions.Logging;

namespace Moviews.Services;

public class MovieService : IMovieService
{
    private readonly IMovieRepository _repository;
    private readonly IUserService _userService;
    private readonly ILogger<MovieService> _logger;

    public MovieService(IMovieRepository repository, IUserService userService, ILogger<MovieService> logger)
    {
        _repository = repository;
        _userService = userService;
        _logger = logger;
    }

    public async Task<IEnumerable<MovieListDTO>> GetAllAsync()
    {
        _logger.LogInformation("Fetching all movies");
        var movies = await _repository.GetAllAsync();
        _logger.LogInformation("Returned {Count} movies", movies.Count());
        return movies;
    }

    public async Task<MovieDTO> GetByIdAsync(Guid id)
    {
        _logger.LogInformation("Fetching movie {Id}", id);
        var movie = await _repository.GetByIdAsync(id);
        if (movie == null) { 
            _logger.LogWarning("Movie {Id} not found", id);
            throw new MovieNotFoundException(id);
        }

        _logger.LogInformation("Movie {MovieId} found successfully", id);
        return movie;
    }

    public async Task<MovieDTO> CreateAsync(MovieInputDTO input)
    {
        _logger.LogInformation("Creating movie {Title}", input.Title);

        var movie = new Movie
        {
            Title = input.Title,
            Genre = input.Genre,
            Synopsis = input.Synopsis,
            Poster = input.Poster,
            ReleaseYear = input.ReleaseYear,
            Deleted = false
        };

        await _repository.AddAsync(movie);
        
        _logger.LogInformation("Movie {Title} created successfully", movie.Title);

        return new MovieDTO
        {
            Id = movie.Id,
            Title = movie.Title,
            Genre = movie.Genre,
            Synopsis = movie.Synopsis,
            Poster = movie.Poster,
            ReleaseYear = movie.ReleaseYear,
            Reviews = new List<ReviewDTO>()
        };

    }

    public async Task<MovieDTO> UpdateAsync(Guid id, MovieInputDTO updatedMovie)
    {
        _logger.LogInformation("Updating movie {MovieId}", id);

        var movieEntity = await _repository.GetEntityByIdAsync(id);
        
        if (movieEntity == null)
        {
            _logger.LogWarning("Movie {Id} not found", id);
            throw new MovieNotFoundException(id);
        }

        movieEntity.Title = updatedMovie.Title;
        movieEntity.Genre = updatedMovie.Genre;
        movieEntity.Synopsis = updatedMovie.Synopsis;
        movieEntity.Poster = updatedMovie.Poster;
        movieEntity.ReleaseYear = updatedMovie.ReleaseYear;

        await _repository.UpdateAsync(movieEntity);

        _logger.LogInformation("Movie {MovieId} updated successfully", id);

        var movieDto = await _repository.GetByIdAsync(id);

        if (movieDto == null)
        {
            _logger.LogError("Movie {MovieId} disappeared after update", id);
            throw new MovieNotFoundException(id);
        }

        return movieDto;
    }

    public async Task<MovieDTO> DeleteAsync(Guid id)
    {
        _logger.LogInformation("Deleting movie {MovieId}", id);

        var movie = await _repository.GetByIdAsync(id);

        if (movie == null)
        {
            _logger.LogWarning("Delete failed. Movie {MovieId} not found", id);
            throw new MovieNotFoundException(id);
        }

        await _repository.DeleteAsync(id);

        _logger.LogInformation("Movie {MovieId} logically deleted", id);

        return movie;
    }

    public async Task<ReviewDTO> AddReviewAsync(Guid movieId, Guid userId, ReviewInputDTO dto)
    {
        _logger.LogInformation("Adding review for Movie {MovieId} by User {UserId}", movieId, userId);

        var movie = await _repository.GetByIdAsync(movieId);
        var user = await _userService.GetUserByIdAsync(userId);

        if (user == null)
        {
            _logger.LogWarning("Review failed. User {UserId} not found", userId);
            throw new UserNotFoundException(userId);
        }

        if (movie == null)
        {
            _logger.LogWarning("Review failed. Movie {MovieId} not found", movieId);
            throw new MovieNotFoundException(movieId);
        }

        if (dto.Rating < 1 || dto.Rating > 5)
        {
            _logger.LogWarning("Invalid rating {Rating} for Movie {MovieId}", dto.Rating, movieId);
            throw new InvalidReviewException("El rating debe estar entre 1 y 5");
        }

        if (string.IsNullOrWhiteSpace(dto.Comment))
        {
            _logger.LogWarning("Empty comment for Movie {MovieId}", movieId);
            throw new InvalidReviewException("El comentario es obligatorio");
        }

        var review = new Review
        {
            MovieId = movieId,
            UserId = userId,
            Rating = dto.Rating,
            Comment = dto.Comment
        };

        var userDTO = new UserDTO
        {
            Id = user.Id,
            UserName = user.UserName,
            PhotoUrl = user.PhotoUrl
        };

        await _repository.AddReviewAsync(review);

        _logger.LogInformation("Review {ReviewId} created for Movie {MovieId}", review.Id, movieId);

        return new ReviewDTO
        {
            Id = review.Id,
            Rating = review.Rating,
            Comment = review.Comment,
            User = userDTO
        };
    }

    public async Task<IEnumerable<ReviewDTO>> GetMovieReviewsAsync(Guid movieId)
    {
        _logger.LogInformation("Fetching reviews for Movie {MovieId}", movieId);

        var reviews = await _repository.GetMovieReviewsAsync(movieId);

        _logger.LogInformation("Returned {Count} reviews for Movie {MovieId}", reviews.Count, movieId);

        return reviews.Select(r => new ReviewDTO
        {
            Id = r.Id,
            Rating = r.Rating,
            Comment = r.Comment,
            User = new UserDTO
            {
                Id = r.User.Id,
                UserName = r.User.UserName,
                PhotoUrl = r.User.PhotoUrl
            }
        });
    }
}
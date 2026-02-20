using Moviews.Exceptions;
using Moviews.Models;
using Moviews.Repositories;

namespace Moviews.Services;

public class MovieService : IMovieService
{
    private readonly IMovieRepository _repository;
    private readonly IUserService _userService;

    public MovieService(IMovieRepository repository, IUserService userService)
    {
        _repository = repository;
        _userService = userService;
    }

    public async Task<IEnumerable<MovieListDTO>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<MovieDTO> GetByIdAsync(Guid id)
    {
        var movie = await _repository.GetByIdAsync(id);
        if (movie == null)
            throw new MovieNotFoundException(id);

        return movie;
    }

    public async Task<MovieDTO> CreateAsync(MovieInputDTO input)
    {
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
        var movieEntity = await _repository.GetEntityByIdAsync(id);
        if (movieEntity == null)
            throw new MovieNotFoundException(id);

        movieEntity.Title = updatedMovie.Title;
        movieEntity.Genre = updatedMovie.Genre;
        movieEntity.Synopsis = updatedMovie.Synopsis;
        movieEntity.Poster = updatedMovie.Poster;
        movieEntity.ReleaseYear = updatedMovie.ReleaseYear;

        await _repository.UpdateAsync(movieEntity);

        var movieDto = await _repository.GetByIdAsync(id);
        return movieDto ?? throw new MovieNotFoundException(id);
    }

    public async Task<MovieDTO> DeleteAsync(Guid id)
    {
        var movie = await _repository.GetByIdAsync(id);
        if (movie == null)
            throw new MovieNotFoundException(id);

        await _repository.DeleteAsync(id);

        return movie;
    }

    public async Task<ReviewDTO> AddReviewAsync(Guid movieId, Guid userId, ReviewInputDTO dto)
    {
        var movie = await _repository.GetByIdAsync(movieId);
        var user = await _userService.GetUserByIdAsync(userId);

        if (user == null)
            throw new UserNotFoundException(userId);

        if (movie == null)
            throw new MovieNotFoundException(movieId);

        if (dto.Rating < 1 || dto.Rating > 5)
            throw new InvalidReviewException("El rating debe estar entre 1 y 5");

        if (string.IsNullOrWhiteSpace(dto.Comment))
            throw new InvalidReviewException("El comentario es obligatorio");

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
        var reviews = await _repository.GetMovieReviewsAsync(movieId);

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
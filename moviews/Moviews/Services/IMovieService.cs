using Moviews.Models;

namespace Moviews.Services;

public interface IMovieService
{
    Task<IEnumerable<MovieListDTO>> GetAllAsync();
    Task<MovieDTO> GetByIdAsync(Guid id);
    Task<MovieDTO> CreateAsync(MovieInputDTO movie);
    Task<MovieDTO> UpdateAsync(Guid id, MovieInputDTO updatedMovie);
    Task<MovieDTO> DeleteAsync(Guid id);
    Task<ReviewDTO> AddReviewAsync(Guid movieId, Guid userId, ReviewInputDTO dto);
    Task<IEnumerable<ReviewDTO>> GetMovieReviewsAsync(Guid movieId);
}
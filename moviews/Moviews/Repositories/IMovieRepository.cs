using Microsoft.EntityFrameworkCore;
using Moviews.Models;

namespace Moviews.Repositories;

public interface IMovieRepository
{
    Task<IEnumerable<MovieListDTO>> GetAllAsync();
    Task<MovieDTO?> GetByIdAsync(Guid id);
    Task<Movie?> GetEntityByIdAsync(Guid id); 
    Task AddAsync(Movie movie);
    Task UpdateAsync(Movie movie);
    Task DeleteAsync(Guid id);
    Task AddReviewAsync(Review review);
    Task<List<Review>> GetMovieReviewsAsync(Guid movieId);
}
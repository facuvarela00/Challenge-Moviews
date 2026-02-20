using Microsoft.EntityFrameworkCore;
using Moviews.Data;
using Moviews.Models;

namespace Moviews.Repositories;

public class MovieRepository : IMovieRepository
{
    private readonly ApplicationDbContext _context;

    public MovieRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MovieListDTO>> GetAllAsync()
    {
        return await _context.Movies
            .Where(m => !m.Deleted)
            .Select(m => new MovieListDTO
            {
                Id = m.Id,
                Title = m.Title,
                Genre = m.Genre,
                ReleaseYear = m.ReleaseYear,
                Synopsis = m.Synopsis,
                Poster = m.Poster
            })
            .ToListAsync();
    }

    public async Task<Movie?> GetEntityByIdAsync(Guid id)
    {
        return await _context.Movies
            .Include(m => m.Reviews)
                .ThenInclude(r => r.User)
            .FirstOrDefaultAsync(m => m.Id == id && !m.Deleted);
    }
    public async Task<MovieDTO?> GetByIdAsync(Guid id)
    {
        var movie = await _context.Movies
            .Where(m => m.Id == id && !m.Deleted)
            .Include(m => m.Reviews)
                .ThenInclude(r => r.User)
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (movie == null) return null;

        var movieDto = new MovieDTO
        {
            Id = movie.Id,
            Title = movie.Title,
            Genre = movie.Genre,
            Synopsis = movie.Synopsis,
            Poster = movie.Poster,
            ReleaseYear = movie.ReleaseYear,
            Reviews = movie.Reviews.Select(r => new ReviewDTO
            {
                Id = r.Id,
                Comment = r.Comment,
                Rating = r.Rating,
                User = new UserDTO
                {
                    Id = r.User.Id,
                    UserName = r.User.UserName,
                    PhotoUrl = r.User.PhotoUrl
                }
            }).ToList()
        };

        return movieDto;
    }

    public async Task AddAsync(Movie movie)
    {
        await _context.Movies.AddAsync(movie);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Movie movie)
    {
        _context.Movies.Update(movie);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var movie = await _context.Movies.FindAsync(id);
        if (movie == null) return;

        movie.Deleted = true;
        await _context.SaveChangesAsync();
    }

    public async Task AddReviewAsync(Review review)
    {
        await _context.Reviews.AddAsync(review);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Review>> GetMovieReviewsAsync(Guid movieId)
    {
        return await _context.Reviews
            .Where(r => r.MovieId == movieId)
            .Include(r => r.User)
            .ToListAsync();
    }
}
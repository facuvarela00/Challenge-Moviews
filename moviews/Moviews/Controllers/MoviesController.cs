using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moviews.Exceptions;
using Moviews.Models;
using Moviews.Services;

[ApiController]
[Route("pelicula")]
public class MoviesController : ControllerBase
{
    private readonly IMovieService _service;

    public MoviesController(IMovieService service)
    {
        _service = service;
    }

    [Authorize(Roles = "Admin,User")]
    [HttpGet("listar")]
    public async Task<IActionResult> GetAll()
    {
        var movies = await _service.GetAllAsync();
        return Ok(movies);
    }

    [Authorize(Roles = "Admin,User")]
    [HttpGet("obtener/{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var movie = await _service.GetByIdAsync(id);
            return Ok(movie);
        }
        catch (MovieNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("crear")]
    public async Task<IActionResult> Create(MovieInputDTO movie)
    {
        var created = await _service.CreateAsync(movie);

        return Ok(created);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("actualizar/{id}")]
    public async Task<IActionResult> Update(Guid id, MovieInputDTO movie)
    {
        try
        {
            var updated = await _service.UpdateAsync(id, movie);
            return Ok(updated);
        }
        catch (MovieNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("borrar/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var deleted = await _service.DeleteAsync(id);
            return Ok(deleted);
        }
        catch (MovieNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(Roles = "User")]
    [HttpPost("{movieId}/review")]
    public async Task<IActionResult> AddReview(Guid movieId, [FromBody] ReviewInputDTO review)
    {
        try
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized();

            var created = await _service.AddReviewAsync( movieId, Guid.Parse(userId), review);

            return Ok(created);
        }
        catch (MovieNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidReviewException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
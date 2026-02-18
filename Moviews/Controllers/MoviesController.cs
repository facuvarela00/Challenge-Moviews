using Microsoft.AspNetCore.Mvc;
using MovieApi.Exceptions;
using MovieApi.Models;
using MovieApi.Services;

namespace MovieApi.Controllers;

[ApiController]
[Route("pelicula")]
public class MoviesController : ControllerBase
{
    private readonly IMovieService _service;

    public MoviesController(IMovieService service)
    {
        _service = service;
    }

    [HttpGet("listar")]
    public IActionResult GetAll()
        => Ok(_service.GetAll());

    [HttpGet("obtener/{id}")]
    public IActionResult GetById(Guid id)
    {
        try
        {
            var movie = _service.GetById(id);
            return Ok(movie);
        }
        catch (MovieNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost("crear")]
    public IActionResult Create(Movie movie)
    {
        if (string.IsNullOrWhiteSpace(movie.Title))
            return BadRequest("Title es obligatorio");

        var created = _service.Create(movie);

        return Ok(created);
    }

    [HttpPut("actualizar/{id}")]
   public IActionResult Update(Guid id, Movie movie)
    {
        try
        {
            _service.Update(id, movie);
            return NoContent();
        }
        catch (MovieNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("borrar/{id}")]
    public IActionResult Delete(Guid id)
    {

        try
        {
           _service.Delete(id);
            return NoContent();
        }
        catch (MovieNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost("review/{movieId}")]
    public IActionResult AddReview(Guid movieId, Review review)
    {
        try
        {
            var created = _service.AddReview(movieId, review);
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

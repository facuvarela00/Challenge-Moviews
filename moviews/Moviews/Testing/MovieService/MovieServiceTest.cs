using Xunit;
using Moq;
using Moviews.Services;
using Moviews.Repositories;
using Moviews.Models;
using Moviews.Exceptions;

public class MovieServiceTests
{
    private readonly Mock<IMovieRepository> _repositoryMock;
    private readonly Mock<IUserService> _userServiceMock;
    private readonly Mock<ILogger<MovieService>> _loggerMock;
    private readonly MovieService _service;

    public MovieServiceTests()
    {
        _repositoryMock = new Mock<IMovieRepository>();
        _userServiceMock = new Mock<IUserService>();
        _loggerMock = new Mock<ILogger<MovieService>>();

        _service = new MovieService(
            _repositoryMock.Object,
            _userServiceMock.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnCreatedMovie()
    {
        var input = new MovieInputDTO
        {
            Title = "Shrek 2",
            Genre = "Comedy",
            Synopsis = "Sinopsis de Shrek 2",
            Poster = "url random",
            ReleaseYear = 2004
        };

        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Movie>()))
            .Returns(Task.CompletedTask);

        var result = await _service.CreateAsync(input);

        Assert.NotNull(result);
        Assert.Equal("Shrek 2", result.Title);

        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Movie>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnMovie_WhenExists()
    {
        var id = Guid.NewGuid();

        _repositoryMock
            .Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync(new MovieDTO
            {
                Id = id,
                Title = "Orogeddon"
            });

        var result = await _service.GetByIdAsync(id);

        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateMovie_WhenExists()
    {
        var id = Guid.NewGuid();

        var entity = new Movie
        {
            Id = id,
            Title = "Una Nueva Esperanza"
        };

        var updatedDto = new MovieInputDTO
        {
            Title = "El Imperio Contraataca",
            Genre = "Action",
            Synopsis = "La mejor pelicula de la historia",
            Poster = "url",
            ReleaseYear = 1980
        };

        _repositoryMock
            .Setup(r => r.GetEntityByIdAsync(id))
            .ReturnsAsync(entity);

        _repositoryMock
            .Setup(r => r.UpdateAsync(entity))
            .Returns(Task.CompletedTask);

        _repositoryMock
            .Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync(new MovieDTO
            {
                Id = id,
                Title = "El Imperio Contraataca"
            });

        var result = await _service.UpdateAsync(id, updatedDto);

        Assert.Equal("El Imperio Contraataca", result.Title);
        _repositoryMock.Verify(r => r.UpdateAsync(entity), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrow_WhenMovieNotFound()
    {
        var id = Guid.NewGuid();

        _repositoryMock
            .Setup(r => r.GetEntityByIdAsync(id))
            .ReturnsAsync((Movie?)null);

        await Assert.ThrowsAsync<MovieNotFoundException>(() =>
            _service.UpdateAsync(id, new MovieInputDTO()));
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnMovie_WhenExists()
    {
        var id = Guid.NewGuid();

        _repositoryMock
            .Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync(new MovieDTO { Id = id });

        _repositoryMock
            .Setup(r => r.DeleteAsync(id))
            .Returns(Task.CompletedTask);

        var result = await _service.DeleteAsync(id);

        Assert.Equal(id, result.Id);
        _repositoryMock.Verify(r => r.DeleteAsync(id), Times.Once);
    }

    [Fact]
    public async Task AddReviewAsync_ShouldThrow_WhenMovieNotFound()
    {
        var movieId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var reviewInput = new ReviewInputDTO
        {
            Rating = 5,
            Comment = "Excelente"
        };

        _repositoryMock
            .Setup(r => r.GetByIdAsync(movieId))
            .ReturnsAsync((MovieDTO?)null);

        _userServiceMock
     .Setup(u => u.GetUserByIdAsync(userId))
     .ReturnsAsync(new User
     {
         Id = userId,
         UserName = "Facu",
         PhotoUrl = "photo.jpg"
     });

        await Assert.ThrowsAsync<MovieNotFoundException>(() =>
            _service.AddReviewAsync(movieId, userId, reviewInput));
    }
}
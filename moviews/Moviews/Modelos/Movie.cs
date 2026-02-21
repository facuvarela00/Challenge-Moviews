using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace Moviews.Models;

public class Movie
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    public string Synopsis { get; set; } = string.Empty;
    public string? Poster { get; set; }
    public int ReleaseYear { get; set; }
    public bool Deleted { get; set; } = false;
    public List<Review> Reviews { get; set; } = new();
}

public class MovieDTO
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    public string Synopsis { get; set; } = string.Empty;
    public string? Poster { get; set; }
    public int ReleaseYear { get; set; }
    public List<ReviewDTO> Reviews { get; set; } = new();
}

public class MovieInputDTO
{
        public string Title { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public string Synopsis { get; set; } = string.Empty;
        public string? Poster { get; set; }
        public int ReleaseYear { get; set; }
}

public class MovieListDTO
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    public string Synopsis { get; set; } = string.Empty;
    public string? Poster { get; set; }
    public int ReleaseYear { get; set; }
}
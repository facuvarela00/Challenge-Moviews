using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace MovieApi.Models;

public class Movie
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Title { get; set; } = string.Empty;

    public string Genre { get; set; } = string.Empty;

    public int ReleaseYear { get; set; }

    public bool Deleted { get; set; } = false;

    public List<Review> Reviews { get; set; } = new();
}

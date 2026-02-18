namespace MovieApi.Models;

public class Review
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid MovieId { get; set; }

    public string Comment { get; set; } = string.Empty;

    public int Rating { get; set; }
}

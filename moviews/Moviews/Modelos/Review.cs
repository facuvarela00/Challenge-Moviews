using Moviews.Models;

public class Review
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid MovieId { get; set; }
    public Movie Movie { get; set; } = null!;
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public string Comment { get; set; } = string.Empty;
    public int Rating { get; set; }
}

public class ReviewDTO
{
    public Guid Id { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
    public UserDTO User { get; set; } = new();
}
public class ReviewInputDTO
{
    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
}




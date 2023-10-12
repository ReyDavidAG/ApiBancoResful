namespace Domain.Entities;

public class RefreshTokenEntity
{
    public Guid IdRefresh { get; set; }
    public string AppUserId { get; set; }
    public string? Token { get; set; }
    public DateTime Created { get; set; }
    public DateTime Expires { get; set; }
    public bool IsExpired => DateTime.Now > Expires;
    public string ReplacedToken { get; set; }
}

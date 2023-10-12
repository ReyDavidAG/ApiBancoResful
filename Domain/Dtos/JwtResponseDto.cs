namespace Domain.Dtos;

public class JwtResponseDto
{
    public string TokenRefreshed { get; set; }
    public DateTime Expires { get; set; }
}

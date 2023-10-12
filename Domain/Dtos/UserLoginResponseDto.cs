namespace Domain.Dtos
{
    public class UserLoginResponseDto
    {
        public string Id { get; set; }
        public UserDatosDto User { get; set; }
        public string Token { get; set; }
        public DateTime? Expires { get; set; }
        public int UserId { get; set; }
        public List<string> Roles { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? refreshExpires { get; set;}
    }
}

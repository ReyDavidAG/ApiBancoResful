namespace Domain.Dtos
{
    public class UserLoginResponseDto
    {
        public UserDatosDto User { get; set; }
        public string Token { get; set; }
    }
}

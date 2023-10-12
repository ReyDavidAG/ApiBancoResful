using System.IdentityModel.Tokens.Jwt;

namespace Domain.Dtos;

public class JwtResponse
{
    public JwtSecurityToken Token { get; set; }
    public DateTime Expires { get; set; }
    public List<string> Roles { get; set; }
}

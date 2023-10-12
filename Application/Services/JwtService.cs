using Application.Interfaces;
using Domain.Dtos;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Services;

public class JwtService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly string secretWord;
    public JwtService(IConfiguration configuration, UserManager<AppUser> userManager)
    {
        _userManager = userManager;
        secretWord = configuration["ApiSettings:Secreta"].ToString();
    }
    public async Task<JwtResponse> CreateJwTokenAsync(AppUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        var managerToken = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(secretWord);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName.ToString()),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault()),
                }),
            Expires = DateTime.UtcNow.AddSeconds(20),
            SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };
        JwtResponse response = new();
        response.Token = (JwtSecurityToken)managerToken.CreateToken(tokenDescriptor);
        response.Expires = (DateTime)tokenDescriptor.Expires;
        response.Roles = roles.ToList();

        return response;
    }
}

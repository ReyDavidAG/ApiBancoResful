using Application.Exceptions;
using Application.Features.Accounts.Commands;
using Application.Features.Accounts.Validators;
using Application.Interfaces;
using Application.Services;
using Application.Wrappers;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;

namespace Application.Features.Accounts.Handlers;

public class JwtRequestHandler : IRequestHandler<JWTRequest, Response<JwtResponseDto>>
{
    private readonly IRepositoryAsync<RefreshTokenEntity> _repositoryRefreshToken;
    private readonly IRepositoryAsync<AppUser> _repositoryAppUser;
    private readonly JwtService _jwtService;
    public JwtRequestHandler(JwtService jwtService, IRepositoryAsync<RefreshTokenEntity> repositoryRefreshToken, IRepositoryAsync<AppUser> repositoryAppUser)
    {
        _repositoryRefreshToken = repositoryRefreshToken;
        _repositoryAppUser = repositoryAppUser;
        _jwtService = jwtService;
    }

    public async Task<Response<JwtResponseDto>> Handle(JWTRequest request, CancellationToken cancellationToken)
    {
        var validator = new JwtRequestValidator();
        var validatorResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validatorResult.IsValid)
            throw new ValidationExceptions(validatorResult.Errors);

        var refreshTokens = await _repositoryRefreshToken.ListAsync();
        var refreshTokenExist = refreshTokens.SingleOrDefault(r => r.Token == request.RefreshToken);
        if (refreshTokenExist == null)
        {
            throw new ApiException($"El {request.RefreshToken} no es valido.");
        }

        var refreshtokenReplaced = refreshTokens.Any(r => r.ReplacedToken == request.RefreshToken);
        if (refreshtokenReplaced)
        {
            throw new ApiException($"El {request.RefreshToken} ya fue refrescado.");
        }
            
        var user = await _repositoryAppUser.GetByIdAsync(refreshTokenExist.AppUserId);

        if (refreshTokenExist.IsExpired)
        {
            RefreshTokenEntity refresh = new()
            {
                IdRefresh = Guid.NewGuid(),
                AppUserId = refreshTokenExist.AppUserId,
                Created = DateTime.Now,
                Expires = DateTime.Now.AddMinutes(2),
                Token = RandomTokenString(),
                ReplacedToken = refreshTokenExist.Token
            };
            var resurltRefresh = await _repositoryRefreshToken.AddAsync(refresh, cancellationToken);
            if (resurltRefresh == null)
                throw new ApiException("Ocurrio un error guardando el RefreshToken");

            JwtResponse tokenresult = await _jwtService.CreateJwTokenAsync(user);
            JwtSecurityToken jwtSecurityToken = tokenresult.Token;

            JwtResponseDto response = new()
            {
                TokenRefreshed = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Expires = tokenresult.Expires,
            };
            return new Response<JwtResponseDto>(response);
        }
        JwtResponse tokenresult2 = await _jwtService.CreateJwTokenAsync(user);
        JwtSecurityToken jwtSecurityToken2 = tokenresult2.Token;

        JwtResponseDto response2 = new()
        {
            TokenRefreshed = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken2),
            Expires = tokenresult2.Expires,
        };
        return new Response<JwtResponseDto>(response2);
    }
    public static string RandomTokenString()
    {
        using var rngCryptoServicesProvider = new RNGCryptoServiceProvider();
        var randomBytes = new byte[40];
        rngCryptoServicesProvider.GetBytes(randomBytes);

        return BitConverter.ToString(randomBytes).Replace("-", "");
    }
}

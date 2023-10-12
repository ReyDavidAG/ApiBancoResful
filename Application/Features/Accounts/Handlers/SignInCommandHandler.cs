using Application.Exceptions;
using Application.Features.Acounts.Commands;
using Application.Features.Acounts.Validators;
using Application.Interfaces;
using Application.Services;
using Application.Wrappers;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Application.Features.Acounts.Handlers
{
    public class SignInCommandHandler : IRequestHandler<SignInCommand, Response<UserLoginResponseDto>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IRepositoryAsync<RefreshTokenEntity> _repositoryRefreshToken;
        private readonly IRepositoryAsync<AppUser> _repositoryAsync;
        private readonly IMapper _mapper;
        private readonly JwtService _jwtServics;
        public SignInCommandHandler(JwtService jwtService,UserManager<AppUser> userManager, IRepositoryAsync<RefreshTokenEntity> repositoryRefreshToken , IRepositoryAsync<AppUser> repositoryAsync, IMapper mapper)
        {
            _userManager = userManager;
            _repositoryRefreshToken = repositoryRefreshToken;
            _repositoryAsync = repositoryAsync;
            _mapper = mapper;
            _jwtServics = jwtService;
        }
        public async Task<Response<UserLoginResponseDto>> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            
            var validator = new SignInCommandValidator(_repositoryAsync);
            var validatorResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validatorResult.IsValid)
                throw new ValidationExceptions(validatorResult.Errors);
        
            var listUsers = await _repositoryAsync.ListAsync();
            var userExist = listUsers.SingleOrDefault(u => u.UserName == request.UserName);
            var correctPass = await _userManager.CheckPasswordAsync(userExist, request.Password);
            if (userExist == null || correctPass == false)
            {
                throw new ApiException($"El usuario o contraseña no son correctos.");
            }

            JwtResponse tokenresult = await _jwtServics.CreateJwTokenAsync(userExist);
            JwtSecurityToken jwtSecurityToken = tokenresult.Token;

            UserLoginResponseDto response = new();
                response.Id = userExist.Id;
                response.Roles = tokenresult.Roles;
                response.User =  _mapper.Map<UserDatosDto>(userExist);
                response.Expires = tokenresult.Expires;
                response.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

                RefreshTokenEntity refresh = new()
                {
                    IdRefresh = Guid.NewGuid(),
                    AppUserId = userExist.Id,
                    Created = DateTime.Now,
                    Expires = DateTime.Now.AddMinutes(2),
                    Token = RandomTokenString(),
                };
                var resurltRefresh = await _repositoryRefreshToken.AddAsync(refresh, cancellationToken);

                if (resurltRefresh != null)
                {
                    response.refreshExpires = resurltRefresh.Expires;
                    response.RefreshToken = resurltRefresh.Token;
                }
 
            return new Response<UserLoginResponseDto>(response);
        }
        public static string RandomTokenString()
        {
            using var rngCryptoServicesProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[40];
            rngCryptoServicesProvider.GetBytes(randomBytes);

            return BitConverter.ToString(randomBytes).Replace("-", "");
        }
    }
}

using Application.Exceptions;
using Application.Features.Acounts.Commands;
using Application.Features.Acounts.Validators;
using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Features.Acounts.Handlers
{
    public class SignInCommandHandler : IRequestHandler<SignInCommand, Response<UserLoginResponseDto>>
    {
        private string claveSecreta;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IRepositoryAsync<AppUser> _repositoryAsync;
        private readonly IMapper _mapper;
        public SignInCommandHandler(IConfiguration config,UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IRepositoryAsync<AppUser> repositoryAsync, IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _repositoryAsync = repositoryAsync;
            _mapper = mapper;
            claveSecreta = config["ApiSettings:Secreta"].ToString();
        }
        public async Task<Response<UserLoginResponseDto>> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            var validator = new SignInCommandValidator(_repositoryAsync);
            var validatorResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validatorResult.IsValid)
                throw new ValidationExceptions(validatorResult.Errors);
        
            var listUsers = _repositoryAsync.ListAsync();
            var userExist = listUsers.Result.SingleOrDefault(u => u.UserName == request.UserName);
            var correctPass = _userManager.CheckPasswordAsync(userExist, request.Password);
            if (userExist == null || correctPass.Result == false)
            {
                throw new ApiException($"El usuario o contraseña no son correctos.");
            }

            var roles = _userManager.GetRolesAsync(userExist);
            var managerToken = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(claveSecreta);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userExist.UserName.ToString()),
                    new Claim(ClaimTypes.Role, roles.Result.FirstOrDefault())
                }),
                Expires = DateTime.UtcNow.AddSeconds(10),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = managerToken.CreateToken(tokenDescriptor); //aqui esta el token para guardarlo

            UserLoginResponseDto userLoginResponseDto = new()
            {
                Token = managerToken.WriteToken(token),
                User = _mapper.Map<UserDatosDto>(userExist),
                Expires = tokenDescriptor.Expires,
            };
            return new Response<UserLoginResponseDto>(userLoginResponseDto);
        }
    }
}

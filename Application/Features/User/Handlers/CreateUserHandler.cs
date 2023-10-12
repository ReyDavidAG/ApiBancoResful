using Application.Exceptions;
using Application.Features.User.Commands;
using Application.Features.User.Validators;
using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Application.Features.User.Handlers
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, Response<UserDatosDto>>
    {
        private readonly IRepositoryAsync<AppUser> _repositoryAsync;
        private readonly IRepositoryAsync<Domain.Entities.User> _repositoryAsync1;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public CreateUserHandler(IRepositoryAsync<AppUser> repositoryAsync, IRepositoryAsync<Domain.Entities.User> repositoryAsync1,  IMapper mapper, RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _repositoryAsync = repositoryAsync;
            _repositoryAsync1 = repositoryAsync1;
            _mapper = mapper;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public async Task<Response<UserDatosDto>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateUserCommandValidator(_repositoryAsync);
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
                throw new ValidationExceptions(validationResult.Errors);
            
            var users = _repositoryAsync.ListAsync();
            var IsUniqueUserName = users.Result.SingleOrDefault(u => u.UserName == request.UserName);

            if (IsUniqueUserName != null)
                throw new ApiException($"El nombre de usuario {request.UserName} ya existe.");

            var IsUniqueEmail = users.Result.SingleOrDefault(u => u.Email == request.Email);

            if (IsUniqueEmail != null)
                throw new ApiException($"El correo electronico {request.Email} ya existe.");
            
            AppUser user = new()
            {
                Nombre = request.Nombre,
                Apellido = request.Apellido,
                Email = request.Email,
                NormalizedEmail = request.UserName.ToUpper(),
                UserName = request.UserName,
            };

            var resultSet = await _userManager.CreateAsync(user, request.Password);

            if (!resultSet.Succeeded)
                throw new ValidationException(resultSet.Errors.ToString());

            var userClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            };
            await _userManager.AddClaimsAsync(user, userClaims);
            if (!_roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
            {
                await _roleManager.CreateAsync(new IdentityRole("admin"));
                await _roleManager.CreateAsync(new IdentityRole("basic"));
            }

            Domain.Entities.User us = new()
            {
                IdAccount = user.Id,
            };
            var value = await _repositoryAsync1.AddAsync(us, cancellationToken);
            await _userManager.AddToRoleAsync(user, request.Role);
                

            var userReturn = _mapper.Map<UserDatosDto>(user);
            return new Response<UserDatosDto>(userReturn);
        }
    }
}

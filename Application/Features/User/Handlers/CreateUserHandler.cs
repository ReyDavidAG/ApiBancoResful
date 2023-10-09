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

namespace Application.Features.User.Handlers
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, Response<UserDatosDto>>
    {
        private readonly IRepositoryAsync<AppUser> _repositoryAsync;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public CreateUserHandler(IRepositoryAsync<AppUser> repositoryAsync, IMapper mapper, RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _repositoryAsync = repositoryAsync;
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
            
            if (!_roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
            {
                await _roleManager.CreateAsync(new IdentityRole("admin"));
                await _roleManager.CreateAsync(new IdentityRole("basic"));
            }

            await _userManager.AddToRoleAsync(user, request.Role);

            var userReturn = _mapper.Map<UserDatosDto>(user);
            return new Response<UserDatosDto>(userReturn);
        }
    }
}

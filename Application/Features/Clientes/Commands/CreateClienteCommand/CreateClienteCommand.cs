using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using FluentValidation;
using Domain.Entities;
using MediatR;
using Application.Features.Clientes.Validators;
using System.Net;
using Application.Exceptions;

namespace Application.Features.Clientes.Commands.CreateClienteCommand
{
    public class CreateClienteCommand : IRequest<Response<int>>
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Direccion { get; set; }
    }
    public class CreateClienteCommandHandler : IRequestHandler<CreateClienteCommand, Response<int>>
    {
        private readonly IRepositoryAsync<Cliente> _repositoryAsync;
        private readonly IMapper _mapper;
        public CreateClienteCommandHandler(IRepositoryAsync<Cliente> repositoryAsync, IMapper mapper)
        {
            _mapper = mapper;
            _repositoryAsync = repositoryAsync;
        }
        public async Task<Response<int>> Handle(CreateClienteCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateClienteCommandValidator(_repositoryAsync);
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var validatorException = new ValidationException(validationResult.Errors);
                throw new ValidationExceptions(validationResult.Errors);
            }
            var nuevoRegistro = _mapper.Map<Cliente>(request);
            var data = await _repositoryAsync.AddAsync(nuevoRegistro, cancellationToken);

            return new Response<int>(data.Id);
        }
    }
}

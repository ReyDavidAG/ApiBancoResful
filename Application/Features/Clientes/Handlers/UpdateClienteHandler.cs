using Application.Exceptions;
using Application.Features.Clientes.Commands;
using Application.Features.Clientes.Validators;
using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Clientes.Handlers
{
    public class UpdateClienteHandler : IRequestHandler<UpdateClienteCommand, Response<int>>
    {
        private readonly IRepositoryAsync<Cliente> _repositoryAsync;
        private readonly IMapper _mapper;
        public UpdateClienteHandler(IRepositoryAsync<Cliente> repositoryAsync, IMapper mapper)
        {
            _repositoryAsync = repositoryAsync;
            _mapper = mapper;
        }
        public async Task<Response<int>> Handle(UpdateClienteCommand request, CancellationToken cancellationToken)
        {
            var cliente = await _repositoryAsync.GetByIdAsync(request.Id);
            if (cliente == null)
            {
                throw new KeyNotFoundException($"Registro no encontrado con el id {request.Id}");
            }
            
            cliente.Nombre = request.Nombre;
            cliente.Apellido = request.Apellido;
            cliente.Telefono = request.Telefono;
            cliente.Direccion = request.Direccion;
            cliente.FechaNacimiento = request.FechaNacimiento;
            cliente.Email = request.Email;

            var validator = new UpdateClienteCommandValidator(_repositoryAsync);
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var validatorException = new ValidationException(validationResult.Errors);
                throw new ValidationExceptions(validationResult.Errors);
            }
            await _repositoryAsync.UpdateAsync(cliente);

            return new Response<int>(cliente.Id);
        }

       
    }
}

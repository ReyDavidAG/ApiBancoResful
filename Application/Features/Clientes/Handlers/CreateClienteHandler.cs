using Application.Exceptions;
using Application.Features.Clientes.Commands;
using Application.Features.Clientes.Validators;
using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.Clientes.Handlers
{
    public class CreateClienteHandler
    {
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
                    throw new ValidationExceptions(validationResult.Errors);
                }
                var nuevoRegistro = _mapper.Map<Cliente>(request);
                var data = await _repositoryAsync.AddAsync(nuevoRegistro, cancellationToken);

                return new Response<int>(data.Id);
            }
        }
    }
}

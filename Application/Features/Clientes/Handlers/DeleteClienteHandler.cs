using Application.Exceptions;
using Application.Features.Clientes.Commands;
using Application.Features.Clientes.Validators;
using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System.Net;

namespace Application.Features.Clientes.Handlers
{
    public class DeleteClienteHandler : IRequestHandler<DeleteClienteCommand, Response<int>>
    {
        private readonly IRepositoryAsync<Cliente> _repositoryAsync;
        private readonly IMapper _mapper;
        public DeleteClienteHandler(IRepositoryAsync<Cliente> repositoryAsync, IMapper mapper)
        {
            _repositoryAsync = repositoryAsync;
            _mapper = mapper;
        }
       
        public async Task<Response<int>> Handle(DeleteClienteCommand request, CancellationToken cancellationToken)
        {
            var cliente = await _repositoryAsync.GetByIdAsync(request.Id);
            if (cliente == null)
            {
                throw new KeyNotFoundException($"Registro no econtrodo con el id {request.Id}");
            }
            await _repositoryAsync.DeleteAsync(cliente);

            return new Response<int>(cliente.Id);
        }
    }
}

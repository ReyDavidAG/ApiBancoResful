using Application.Features.Clientes.Queries;
using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Clientes.Handlers
{
    public class GetClienteByIdHandler : IRequestHandler<GetClienteByIdQuery, Response<ClienteDto>>
    {
        private readonly IRepositoryAsync<Cliente> _repositoryAsync;
        private readonly IMapper _mapper;
        public GetClienteByIdHandler(IRepositoryAsync<Cliente> repositoryAsync, IMapper mapper)
        {
            _repositoryAsync = repositoryAsync;
            _mapper = mapper;
        }
         public async Task<Response<ClienteDto>> Handle(GetClienteByIdQuery request, CancellationToken cancellationToken)
        {
            var cliente = await _repositoryAsync.GetByIdAsync(request.Id);
            if (cliente == null)
            {
                throw new KeyNotFoundException($"Registro no encontrado con el id {request.Id}");
            }
            var result = _mapper.Map<ClienteDto>(cliente);

            return new Response<ClienteDto>(result);
        }
    }
}

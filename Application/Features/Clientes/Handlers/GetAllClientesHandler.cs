using Application.Features.Clientes.Queries;
using Application.Interfaces;
using Application.Specifications;
using Application.Wrappers;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Clientes.Handlers
{
    public class GetAllClientesHandler : IRequestHandler<GetAllClientesQuery, PagedResponse<List<ClienteDto>>>
    {
        private readonly IRepositoryAsync<Cliente> _repositoryAsync;
        private readonly IMapper _mapper;
        public GetAllClientesHandler(IRepositoryAsync<Cliente> repositoryAsync, IMapper mapper)
        {
            _mapper = mapper;
            _repositoryAsync = repositoryAsync;
        }
        public async Task<PagedResponse<List<ClienteDto>>> Handle(GetAllClientesQuery request, CancellationToken cancellationToken)
        {
            var clientes = await _repositoryAsync.ListAsync(new PagedClientesSpecification(request.PageSize, request.PageNumber, request.Nombre, request.Apellido));
            var clientesDto = _mapper.Map<List<ClienteDto>>(clientes);
            return new PagedResponse<List<ClienteDto>>(clientesDto, request.PageNumber, request.PageSize);
        }
    }
} 

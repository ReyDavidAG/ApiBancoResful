using Application.Features.Clientes.Queries;
using Application.Interfaces;
using Application.Specifications;
using Application.Wrappers;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Features.Clientes.Handlers
{
    public class GetAllClientesHandler : IRequestHandler<GetAllClientesQuery, PagedResponse<List<ClienteDto>>>
    {
        private readonly IRepositoryAsync<Cliente> _repositoryAsync;
        private readonly IDistributedCache _distributedCache; 
        private readonly IMapper _mapper;
        public GetAllClientesHandler(IRepositoryAsync<Cliente> repositoryAsync, IMapper mapper, IDistributedCache distributedCache)
        {
            _mapper = mapper;
            _distributedCache = distributedCache;
            _repositoryAsync = repositoryAsync;
        }
        public async Task<PagedResponse<List<ClienteDto>>> Handle(GetAllClientesQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"listadoClientes_{request.PageSize}_{request.PageNumber}_{request.Nombre}_{request.Apellido}";
            string serializedListClientes;
            var listClientes = new List<Cliente>();
            var redisListClientes = await _distributedCache.GetAsync(cacheKey);
            if (redisListClientes != null)
            {
                serializedListClientes = Encoding.UTF8.GetString(redisListClientes);
                listClientes = JsonConvert.DeserializeObject<List<Cliente>>(serializedListClientes);
            }
            else
            {
                listClientes = await _repositoryAsync.ListAsync(new PagedClientesSpecification(request.PageSize, request.PageNumber, request.Nombre, request.Apellido));
                serializedListClientes = JsonConvert.SerializeObject(listClientes);
                redisListClientes = Encoding.UTF8.GetBytes(serializedListClientes);
                    
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));

                await _distributedCache.SetAsync(cacheKey, redisListClientes, options);
            }
            var clientesDto = _mapper.Map<List<ClienteDto>>(listClientes);
            return new PagedResponse<List<ClienteDto>>(clientesDto, request.PageNumber, request.PageSize);
        }
    }
} 

using Application.Features.Users.Queries;
using Application.Interfaces;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Application.Features.Users.Handlers
{
    public class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, List<UserDatosDto>>
    {
        private readonly IRepositoryAsync<AppUser> _repositoryAsync;
        private readonly IMapper _mapper;
        public GetAllUsersHandler(IRepositoryAsync<AppUser> repositoryAsync, IMapper mapper)
        {
            _repositoryAsync = repositoryAsync;
            _mapper = mapper;
        }

        public async Task<List<UserDatosDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _repositoryAsync.ListAsync();
            var listfiltered = new List<UserDatosDto>();
            if (users.Count == 0)
                throw new KeyNotFoundException("No se encontraron usuarios.");
            
            if (request.Nombre == null)
                return _mapper.Map<List<UserDatosDto>>(users);
            else
            {
                foreach (var user in users)
                {
                    if (user.Nombre.Contains(request.Nombre, StringComparison.OrdinalIgnoreCase))
                    {
                        listfiltered.Add(_mapper.Map<UserDatosDto>(user));
                    }

                }
                if (listfiltered.Count == 0)
                {
                    throw new KeyNotFoundException($"No se encontraron usuarios con el nombre {request.Nombre}.");
                }
                return listfiltered;
            }
        }
    }
}

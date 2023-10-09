using Application.Features.Users.Queries;
using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using MediatR;

namespace Application.Features.Users.Handlers
{
    public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, Response<UserDatosDto>>
    {
        private readonly IRepositoryAsync<AppUser> _repositoryAsync;
        private readonly IMapper _mapper;
        public GetUserByIdHandler(IRepositoryAsync<AppUser> repositoryAsync, IMapper mapper)
        {
            _repositoryAsync = repositoryAsync;
            _mapper = mapper;
        }
        public async Task<Response<UserDatosDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _repositoryAsync.GetByIdAsync(request.Id);

            if (user == null)
                throw new KeyNotFoundException($"Regustro no encontrado con el id {request.Id}");

            var result = _mapper.Map<UserDatosDto>(user);

            return new Response<UserDatosDto>(result);
        }
    }
}

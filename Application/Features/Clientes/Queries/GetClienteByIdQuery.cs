using Application.Wrappers;
using Domain.Dtos;
using Domain.Entities;
using MediatR;

namespace Application.Features.Clientes.Queries
{
    public class GetClienteByIdQuery : IRequest<Response<ClienteDto>>
    {
        public int Id { get; set; }
    }
}

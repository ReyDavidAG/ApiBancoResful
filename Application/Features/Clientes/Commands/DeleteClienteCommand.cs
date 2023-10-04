using Application.Wrappers;
using MediatR;

namespace Application.Features.Clientes.Commands
{
    public class DeleteClienteCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
    }
}

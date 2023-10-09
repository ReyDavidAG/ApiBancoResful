using Domain.Dtos;
using MediatR;

namespace Application.Features.Users.Queries
{
    public class GetAllUsersQuery : IRequest<List<UserDatosDto>>
    {
        public string? Nombre { get; set; }
    }
}

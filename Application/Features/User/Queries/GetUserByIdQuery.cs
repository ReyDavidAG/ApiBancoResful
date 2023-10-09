using Application.Wrappers;
using Domain.Dtos;
using MediatR;

namespace Application.Features.Users.Queries
{
    public class GetUserByIdQuery : IRequest<Response<UserDatosDto>>
    {
        public string Id { get; set; }
    }
}

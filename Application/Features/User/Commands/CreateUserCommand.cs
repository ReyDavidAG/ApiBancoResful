

using Application.Wrappers;
using Domain.Dtos;
using MediatR;

namespace Application.Features.User.Commands
{
    public class CreateUserCommand : IRequest<Response<UserDatosDto>>
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email{ get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}

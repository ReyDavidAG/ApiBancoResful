using Application.Wrappers;
using Domain.Dtos;
using MediatR;

namespace Application.Features.Acounts.Commands
{
    public class SignInCommand : IRequest<Response<UserLoginResponseDto>>
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
    }
}

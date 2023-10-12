using Application.Wrappers;
using Domain.Dtos;
using MediatR;

namespace Application.Features.Accounts.Commands;

public class JWTRequest : IRequest<Response<JwtResponseDto>>
{
    public string RefreshToken { get; set; }
}

using Application.Wrappers;
using Domain.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Acounts.Commands
{
    public class SignInCommand : IRequest<Response<UserLoginResponseDto>>
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
    }
}

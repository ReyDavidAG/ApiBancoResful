using Application.Features.User.Commands;
using Application.Features.Users.Queries;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.v1
{
    [ApiVersion("1.0")]
    public class UsersController : BaseAPIController
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllUsersQuery filter)
        {
            return Ok(await Mediator.Send(new GetAllUsersQuery { Nombre = filter.Nombre }));
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            return Ok(await Mediator.Send(new GetUserByIdQuery { Id = id }));
        }
        [HttpPost]
        public async Task<IActionResult> Post(CreateUserCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
    }
}

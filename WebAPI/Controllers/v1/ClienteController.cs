using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Application.Features.Clientes.Commands;
using Application.Features.Clientes.Queries;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize(Roles = "basic")]
    public class ClienteController : BaseAPIController
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery] GetAllClientesParameters filter)
        {
            return Ok(await Mediator.Send(new GetAllClientesQuery { PageNumber = filter.PageNumber, PageSize = filter.PageSize, Nombre = filter.Nombre, Apellido = filter.Apellido }));
        }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await Mediator.Send(new GetClienteByIdQuery { Id = id }));
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Post(CreateClienteCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Put(int id, UpdateClienteCommand command)
        {
            if (id != command.Id)
                return BadRequest();

            return Ok(await Mediator.Send(command));
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeleteClienteCommand { Id = id }));
        }
    }
}

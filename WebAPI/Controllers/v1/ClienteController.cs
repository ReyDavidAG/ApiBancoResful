using Application.Features.Clientes.Commands.CreateClienteCommand;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.v1
{
    [ApiVersion("1.0")]
    public class ClienteController : BaseAPIController
    {
        [HttpPost]
        public async Task<IActionResult> Post(CreateClienteCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
    }
}

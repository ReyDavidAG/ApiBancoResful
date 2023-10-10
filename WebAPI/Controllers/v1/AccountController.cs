using Application.Features.Acounts.Commands;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.v1
{
    [ApiVersion("1.0")]
    public class AccountController : BaseAPIController
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SignInCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
    }
}

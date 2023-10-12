using Application.Features.Accounts.Commands;
using Application.Features.Acounts.Commands;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.v1
{
    [ApiVersion("1.0")]
    public class AccountController : BaseAPIController
    {
        [HttpPost("Login")]
        public async Task<IActionResult> Post([FromBody] SignInCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
        [HttpPost("RefreshToken")]
        public async Task<IActionResult> Refresh([FromBody] JWTRequest request)
        {
            return Ok(await Mediator.Send(request));
        }
    }
}

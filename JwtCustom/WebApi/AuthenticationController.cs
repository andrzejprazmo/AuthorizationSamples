using FluentValidation;
using JwtCustom.Application.Commands.Authenticate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JwtCustom.WebApi
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMediator mediator;

        public AuthenticationController(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [Route("authenticate")]
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthenticateResponse), 200)]
        public async Task<IActionResult> Authenticate(AuthenticateRequest request)
        {
            var result = await mediator.Send(request);
            return result.Match(data => Ok(data), fail =>
            {
                if (fail is ValidationException exception)
                {
                    return BadRequest(exception.Message);
                }
                return StatusCode(StatusCodes.Status500InternalServerError, "System Error");
            });
        }
    }
}

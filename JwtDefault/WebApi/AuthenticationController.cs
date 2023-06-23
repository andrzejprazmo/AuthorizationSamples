using FluentValidation;
using JwtDefault.Application.Commands.Authenticate;
using JwtDefault.Application.Commands.RefreshToken;
using JwtDefault.Application.Common.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JwtDefault.WebApi
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

        [HttpGet]
        [Route("refresh-token")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(RefreshTokenResponse), 200)]
        public async Task<IActionResult> RefreshToken()
        {
            var result = await mediator.Send(new RefreshTokenRequest());
            return result.Match(data => Ok(data), fail =>
            {
                if (fail is TokenExpiredException exception)
                {
                    return Unauthorized(exception.Message);
                }
                return StatusCode(StatusCodes.Status500InternalServerError, "System Error");
            });
        }
    }
}

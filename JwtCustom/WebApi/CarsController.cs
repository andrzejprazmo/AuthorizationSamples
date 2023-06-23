using FluentValidation;
using JwtCustom.Application.Commands.CreateCar;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JwtCustom.WebApi
{
    [Route("api/cars")]
    [Authorize]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly IMediator mediator;

        public CarsController(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(Guid), 200)]
        public async Task<IActionResult> Create(CreateCarRequest request) 
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

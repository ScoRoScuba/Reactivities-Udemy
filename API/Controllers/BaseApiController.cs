using Microsoft.AspNetCore.Mvc;
using Application.Core;
using MediatR;

namespace API.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
        protected readonly IMediator _mediator;

        public BaseApiController(IMediator mediator)
        {
            _mediator = mediator;
        }

        protected ActionResult HandleResult<T>(Result<T> result)
        {
            if (result == null)
                return NotFound();
            if (result.IsSuccess && result.Value != null)
                return Ok(result.Value);
            if (result.IsSuccess && result.Value == null)
                return NotFound();

            return BadRequest(result.Error);
        }

    }
}

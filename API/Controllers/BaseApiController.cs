using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace API.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class BAseApiController : ControllerBase
    {
        protected readonly IMediator _mediator;

        public BAseApiController(IMediator mediator)
        {
            _mediator = mediator;
        }
    }
}

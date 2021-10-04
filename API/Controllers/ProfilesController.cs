using System;
using System.Threading.Tasks;
using Application.Profiles;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProfilesController : BaseApiController
    {
        public ProfilesController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> GetProfile(string username)
        {
            var result = await _mediator.Send(new Details.Query(username));
            return HandleResult(result);
        }

        [HttpPut]
        public async Task<IActionResult> EditActivity(Profile profile)
        {
            var result = await _mediator.Send(new Edit.Command(profile));

            return HandleResult(result);
        }

        [HttpGet("{username}/activities")]
        public async Task<IActionResult> GetUserActivities(string username, string predicate)
        {
            var result = await _mediator.Send(new ListActivities.Query(username, predicate));
            return HandleResult(result);
        }
    }
}

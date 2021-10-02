using System.Threading.Tasks;
using Application.Followers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class FollowController: BaseApiController
    {
        public FollowController(IMediator mediator) : base(mediator)
        {}

        [HttpPost("{username}")]
        public async Task<IActionResult> Follow(string username)
        {
            var result = await _mediator.Send(new FollowToggle.Command(username));
            return HandleResult(result);
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> Followings(string username, string predicate)
        {
            var result = await _mediator.Send(new List.Query(predicate, username));
            return HandleResult(result);
        }


    }
}

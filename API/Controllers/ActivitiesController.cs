using System;
using System.Threading.Tasks;
using Application.Activities;
using Domain;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [AllowAnonymous]
    public class ActivitiesController : BaseApiController
    {
        public ActivitiesController(IMediator mediator) : base(mediator) { }

        [HttpGet]
        public async Task<IActionResult> GetActivities()
        {
            var result = await _mediator.Send(new List.Query());
            return HandleResult(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetActivity(Guid id)
        {
            var result = await _mediator.Send(new Details.Query(id));
            return HandleResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateActivity(Activity activity)
        {
            var result =await _mediator.Send(new Create.Command(activity));
            return HandleResult(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditActivity(Guid id, Activity activity)
        {
            activity.Id = id;

            var result = await _mediator.Send(new Edit.Command(activity));
            return HandleResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivity(Guid id)
        {   
            var result = await _mediator.Send(new Delete.Command(id));
            return HandleResult(result);
        }
    }
}

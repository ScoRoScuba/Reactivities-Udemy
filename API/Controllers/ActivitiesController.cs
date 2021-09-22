using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Activities;
using Domain;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Cors;

namespace API.Controllers
{
    public class ActivitiesController : BAseApiController
    {
        public ActivitiesController(IMediator mediator) : base(mediator) { }

        [HttpGet]
        public async Task<ActionResult<List<Activity>>> GetActivities()
        {
            return await _mediator.Send(new List.Query());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Activity>> GetActivity(Guid id)
        {
            return await _mediator.Send(new Details.Query(id));
        }

        [HttpPost]
        public async Task<IActionResult> CreateActivity(Activity activity)
        {
            var result =await _mediator.Send(new Create.Command(activity));

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditActivity(Guid id, Activity activity)
        {
            activity.Id = id;

            var result = await _mediator.Send(new Edit.Command(activity));

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivity(Guid id)
        {   
            var result = await _mediator.Send(new Delete.Command(id));

            return Ok(result);
        }
    }
}

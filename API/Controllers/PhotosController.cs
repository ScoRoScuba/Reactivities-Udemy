using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Application.Photos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace API.Controllers
{
    public class PhotosController : BaseApiController
    {
        public PhotosController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm]Add.Command command)
        {
            var result = await _mediator.Send(command);
            return HandleResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _mediator.Send(new Delete.Command(id));
            return HandleResult(result);
        }

        [HttpPost("{id}/setMain")]
        public async Task<IActionResult> SetMain(string id)
        {
            var result = await _mediator.Send(new SetMain.Command(id));
            return HandleResult(result);
        }
    }
}

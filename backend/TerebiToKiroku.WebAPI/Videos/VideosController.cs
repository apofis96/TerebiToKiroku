using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TerebiToKiroku.Application.Videos;
using TerebiToKiroku.Application.Videos.StartWatchVideo;

namespace TerebiToKiroku.WebAPI.Videos
{
    [Route("api/videos")]
    [ApiController]
    public class VideosController : Controller
    {
        private readonly IMediator _mediator;

        public VideosController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [Route("")]
        [HttpPost]
        [ProducesResponseType(typeof(VideoDto), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> RegisterCustomer([FromBody] StartWatchVideoRequest request)
        {
            var video = await _mediator.Send(new StartWatchVideoCommand(request.Name, request.Key, request.Duration));

            return Created(string.Empty, video);
        }
    }
}
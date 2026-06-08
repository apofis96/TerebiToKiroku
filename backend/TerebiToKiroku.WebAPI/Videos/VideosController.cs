using MediatR;
using Microsoft.AspNetCore.Mvc;

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

        /*[Route("{customerId}/orders")]
        [HttpGet]
        [ProducesResponseType(typeof(List<OrderDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCustomerOrders(Guid customerId)
        {
            var orders = await _mediator.Send(new GetCustomerOrdersQuery(customerId));

            return Ok(orders);
        }

        [Route("{customerId}/orders")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> AddCustomerOrder(
            [FromRoute] Guid customerId,
            [FromBody] CustomerOrderRequest request)
        {
            await _mediator.Send(new PlaceCustomerOrderCommand(customerId, request.Products, request.Currency));

            return Created(string.Empty, null);
        }*/
    }
}
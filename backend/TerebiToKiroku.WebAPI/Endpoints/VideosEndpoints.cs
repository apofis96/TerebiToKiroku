using Mediator;
using Microsoft.AspNetCore.Mvc;
using TerebiToKiroku.Application;
using TerebiToKiroku.Application.Videos.StartWatchVideo;

namespace TerebiToKiroku.WebAPI.Endpoints
{
    public static class VideosEndpoints
    {
        public static void MapVideosEndpoints(this WebApplication app)
        {
            var videosApi = app.MapGroup("/videos").RequireAuthorization();
            videosApi.MapGet("/", async (IMediator mediator, CancellationToken cancellationToken) =>
            {
                return Results.Ok();
            }).WithName("Get Videos");

            videosApi.MapPost("/start-watch", async ([FromBody] StartWatchVideoRequest request, IMediator mediator, CancellationToken cancellationToken) =>
            {
                try
                {
                    var command = new StartWatchVideoCommand();
                    var response = await mediator.Send(command, cancellationToken);
                    return Results.Ok(response);
                }
                catch (ValidationException ex)
                {
                    return Results.BadRequest(ex.ValidationError);
                }
            }).WithName("Start Watch Video");
        }
    }
}

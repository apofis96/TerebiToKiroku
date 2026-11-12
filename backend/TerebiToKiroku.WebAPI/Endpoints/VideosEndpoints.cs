using Mediator;
using Microsoft.AspNetCore.Mvc;
using TerebiToKiroku.Application.Videos.StartWatchVideo;
using TerebiToKiroku.Domain.Exceptions;

namespace TerebiToKiroku.WebAPI.Endpoints
{
    public static class VideosEndpoints
    {
        public static void MapVideosEndpoints(this WebApplication app)
        {
            var videosApi = app.MapGroup("/videos").RequireAuthorization();
            videosApi.MapPost("/start-watch", async ([FromBody] StartWatchVideoRequest request, IMediator mediator, CancellationToken cancellationToken) =>
            {
                try
                {
                    var command = new StartWatchVideoCommand()
                    {
                        Key = request.Key,
                        Name = request.Name,
                        Duration = request.Duration
                    };

                    var response = await mediator.Send(command, cancellationToken);
                    return Results.Ok(response.Id);
                }
                catch (ValidationException ex)
                {
                    return Results.BadRequest(ex.ValidationError);
                }
                catch (BusinessRuleValidationException ex)
                {
                    return Results.BadRequest(ex.BrokenRule);
                }
            }).WithName("Start Watch Video");
        }
    }
}

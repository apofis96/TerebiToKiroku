using Mediator;
using Microsoft.AspNetCore.Mvc;
using TerebiToKiroku.Application.WatchSessions.TrackWatchSession;
using TerebiToKiroku.Domain.Exceptions;

namespace TerebiToKiroku.WebAPI.Endpoints
{
    public static class WatchSessionsEndpoints
    {
        public static void MapWatchSessionsEndpoints(this WebApplication app)
        {
            var watchSessionsApi = app.MapGroup("/watch-sessions").RequireAuthorization();
            watchSessionsApi.MapPost("/track", async ([FromBody] TrackWatchSessionRequest request, IMediator mediator, CancellationToken cancellationToken) =>
            {
                try
                {
                    var command = new TrackWatchSessionCommand()
                    {
                        Key = request.Key,
                        SessionDuration = request.SessionDuration,
                        WatchedDuration = request.WatchedDuration
                    };

                    var response = await mediator.Send(command, cancellationToken);
                    return Results.Ok();
                }
                catch (ValidationException ex)
                {
                    return Results.BadRequest(ex.ValidationError);
                }
                catch (BusinessRuleValidationException ex)
                {
                    return Results.BadRequest(ex.BrokenRule);
                }
            }).WithName("Track Watch Session");
        }
    }
}

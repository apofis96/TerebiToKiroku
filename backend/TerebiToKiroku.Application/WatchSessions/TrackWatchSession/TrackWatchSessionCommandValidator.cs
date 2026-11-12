using FluentValidation;

namespace TerebiToKiroku.Application.WatchSessions.TrackWatchSession
{
    public class TrackWatchSessionCommandValidator : AbstractValidator<TrackWatchSessionCommand>
    {
        public TrackWatchSessionCommandValidator()
        {
            RuleFor(x => x.Key).NotEmpty();
            RuleFor(x => x.SessionDuration).GreaterThanOrEqualTo(0);
            RuleFor(x => x.WatchedDuration).GreaterThanOrEqualTo(0);
        }
    }
}

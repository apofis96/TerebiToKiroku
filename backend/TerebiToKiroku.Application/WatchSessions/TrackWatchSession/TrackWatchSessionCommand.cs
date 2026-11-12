using Mediator;
using System.Diagnostics.CodeAnalysis;
using TerebiToKiroku.Application.Interfaces;
using TerebiToKiroku.Domain.Abstractions;

namespace TerebiToKiroku.Application.WatchSessions.TrackWatchSession
{
    public class TrackWatchSessionCommand: ICommand, IValidate
    {
        public required string Key { get; set; }
        public required int SessionDuration { get; set; }
        public required int WatchedDuration { get; set; }

        public bool IsValid([NotNullWhen(false)] out ValidationError? error)

        {
            var validator = new TrackWatchSessionCommandValidator();
            var result = validator.Validate(this);
            error = result.IsValid ? null : new ValidationError([.. result.Errors.Select(e => e.ErrorMessage)]);

            return result.IsValid;
        }
    }
}

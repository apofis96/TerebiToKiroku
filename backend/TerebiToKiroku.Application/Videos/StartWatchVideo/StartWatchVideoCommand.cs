using Mediator;
using System.Diagnostics.CodeAnalysis;
using TerebiToKiroku.Domain.Entities;

namespace TerebiToKiroku.Application.Videos.StartWatchVideo
{
    public sealed record StartWatchVideoCommand : ICommand<Video>, IValidate
    {
        public required string Name { get; set; }

        public required string Key { get; set; }

        public required int Duration { get; set; }

        public bool IsValid([NotNullWhen(false)] out ValidationError? error)
        
        {
            var validator = new StartWatchVideoCommandValidator();
            var result = validator.Validate(this);
            error = result.IsValid ? null : new ValidationError([.. result.Errors.Select(e => e.ErrorMessage)]);
   
            return result.IsValid;
        }
    }
}

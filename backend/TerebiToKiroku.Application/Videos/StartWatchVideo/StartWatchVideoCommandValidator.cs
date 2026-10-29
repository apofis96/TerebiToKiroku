using FluentValidation;

namespace TerebiToKiroku.Application.Videos.StartWatchVideo
{
    public class StartWatchVideoCommandValidator : AbstractValidator<StartWatchVideoCommand>
    {
        public StartWatchVideoCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Key).NotEmpty();
            RuleFor(x => x.Duration).GreaterThan(0);
        }
    }
}

using FluentValidation;

namespace TerebiToKiroku.Application.Videos.StartWatchVideo
{
    public class StartWatchVideoCommandValidator : AbstractValidator<StartWatchVideoCommand>
    {
        public StartWatchVideoCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is empty");
            RuleFor(x => x.Duration).NotEmpty().WithMessage("Duration is empty")
                .GreaterThan(0).WithMessage("Duration must be a positive number");
            RuleFor(x => x.Key).NotEmpty().WithMessage("Key is empty");
        }
    }
}
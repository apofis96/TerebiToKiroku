using TerebiToKiroku.Domain.Abstractions;

namespace TerebiToKiroku.Domain.Exceptions
{
    public sealed class ValidationException(ValidationError validationError) : Exception("Validation error")
    {
        public ValidationError ValidationError { get; } = validationError;
    }
}

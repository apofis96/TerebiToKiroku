namespace TerebiToKiroku.Application
{
    public sealed class ValidationException(ValidationError validationError) : Exception("Validation error")
    {
        public ValidationError ValidationError { get; } = validationError;
    }
}

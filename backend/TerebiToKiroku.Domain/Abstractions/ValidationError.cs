namespace TerebiToKiroku.Domain.Abstractions
{
    public sealed record ValidationError(IEnumerable<string> Errors);
}

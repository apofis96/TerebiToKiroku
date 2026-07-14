namespace TerebiToKiroku.Application
{
    public sealed record ValidationError(IEnumerable<string> Errors);
}

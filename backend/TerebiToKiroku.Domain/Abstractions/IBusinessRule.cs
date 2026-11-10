namespace TerebiToKiroku.Domain.Abstractions
{
    public interface IBusinessRule
    {
        bool IsBroken { get; }
        string Message { get; }
    }
}

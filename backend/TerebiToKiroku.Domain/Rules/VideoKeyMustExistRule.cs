using TerebiToKiroku.Domain.Abstractions;

namespace TerebiToKiroku.Domain.Rules
{
    public sealed class VideoKeyMustExistRule(bool isUnique) : IBusinessRule
    {
        public bool IsBroken => !isUnique;

        public string Message => "Video key must exist.";
    }
}

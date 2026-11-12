using TerebiToKiroku.Domain.Abstractions;

namespace TerebiToKiroku.Domain.Rules
{
    public sealed class VideoKeyMustBeUniqueRule(bool isUnique) : IBusinessRule
    {
        public bool IsBroken => !isUnique;

        public string Message => "Video key must be unique.";
    }
}

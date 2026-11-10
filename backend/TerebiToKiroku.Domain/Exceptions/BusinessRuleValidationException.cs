using TerebiToKiroku.Domain.Abstractions;

namespace TerebiToKiroku.Domain.Exceptions
{
    public sealed class BusinessRuleValidationException(IBusinessRule brokenRule) : Exception(brokenRule.Message)
    {
        public IBusinessRule BrokenRule { get; } = brokenRule;
    }
}

using TerebiToKiroku.Domain.Exceptions;

namespace TerebiToKiroku.Domain.Abstractions
{
    public abstract class Entity
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }

        protected static void CheckRule(IBusinessRule rule)
        {
            if (rule.IsBroken)
                throw new BusinessRuleValidationException(rule);
        }
    }
}

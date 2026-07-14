using Mediator;
using System.Diagnostics.CodeAnalysis;

namespace TerebiToKiroku.Application
{
    public interface IValidate : IMessage
    {
        bool IsValid([NotNullWhen(false)] out ValidationError? error);
    }
}

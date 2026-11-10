using Mediator;
using System.Diagnostics.CodeAnalysis;
using TerebiToKiroku.Domain.Abstractions;

namespace TerebiToKiroku.Application.Interfaces
{
    public interface IValidate : IMessage
    {
        bool IsValid([NotNullWhen(false)] out ValidationError? error);
    }
}

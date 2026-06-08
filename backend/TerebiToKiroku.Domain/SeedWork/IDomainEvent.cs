using System;
using MediatR;

namespace TerebiToKiroku.Domain.SeedWork
{
    public interface IDomainEvent : INotification
    {
        DateTime OccurredOn { get; }
    }
}
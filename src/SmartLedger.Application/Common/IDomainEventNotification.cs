using MediatR;
using SmartLedger.Domain.Common;

namespace SmartLedger.Application.Common;

public interface IDomainEventNotification<TDomainEvent> : INotification
    where TDomainEvent : IDomainEvent
{
    TDomainEvent DomainEvent { get; }
}
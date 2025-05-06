using MediatR;

namespace EShop.Shared.Domain;

public interface IDomainEvent: INotification
{
    Guid EventId => Guid.NewGuid();
    public DateTime CreatedAt => DateTime.Now;
    public string EventType => GetType().AssemblyQualifiedName!;
}
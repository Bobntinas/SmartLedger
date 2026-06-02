namespace SmartLedger.Domain.Common;

public abstract class AggregateRoot : Entity
{
    public uint Version { get; protected set; }
}
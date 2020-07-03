using System;
using NServiceBus;

namespace EndPoint.Sagas
{
    public class BridgeAggregateWithId<TAggregate, TId> : ContainSagaData
        where TAggregate : Domain.AggregateRoot<TId>
    {
        public TId AggregateRootId { get; set; }
        public TAggregate AggregateRoot { get; set; }
    }

    public class BridgeAggregate<TAggregate> : BridgeAggregateWithId<TAggregate, Guid>
        where TAggregate : Domain.AggregateRoot<Guid>
    {
        public class WithId<TId> : BridgeAggregateWithId<TAggregate, Guid> { }
    }
}
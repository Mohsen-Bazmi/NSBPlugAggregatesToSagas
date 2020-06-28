using System;
using Domain;
using NServiceBus;

namespace EndPoint.Sagas
{
    public abstract class WrapTheAggregate<T> : ContainSagaData
        where T : Domain.AggregateRoot
    {
        public Guid AggregateRootId
        {
            get => AggregateRoot.Id;
            set => AggregateRoot = CreateAggregateRoot(value);
        }
        protected abstract T CreateAggregateRoot(Guid id);
        public T AggregateRoot { get; set; }
    }
}
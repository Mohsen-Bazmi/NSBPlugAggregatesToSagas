using System;
using System.Collections.Generic;

namespace Domain
{
    public abstract class AggregateRoot
    {
        List<object> newEvents { get; } = new List<object>();
        public IReadOnlyCollection<object> NewEvents => newEvents;

        protected void AppendEvent(object @event)
        => newEvents.Add(@event);
        public void ForgetNewEvents()
        => newEvents.RemoveAll(_ => true);
    }
    public abstract class AggregateRoot<TId> : AggregateRoot
    {
        public AggregateRoot(TId id)
        {
            Id = id;
        }
        public TId Id { get; }
    }
}
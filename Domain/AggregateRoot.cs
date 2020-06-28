using System;
using System.Collections.Generic;

namespace Domain
{
    public abstract class AggregateRoot
    {
        public AggregateRoot(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; }
        List<object> newEvents { get; set; } = new List<object>();
        public IReadOnlyCollection<object> NewEvents => newEvents;

        protected void AppendEvent(object @event)
        => newEvents.Add(@event);
        public void ForgetNewEvents()
        => newEvents.RemoveAll(_ => true);
    }
}
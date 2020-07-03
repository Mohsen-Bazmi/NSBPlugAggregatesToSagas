using System;
using System.Threading.Tasks;
using Commands;
using Domain;
using EndPoint.NServiceBusExtensions;
using NServiceBus;

namespace EndPoint.Sagas
{
    public class UserSaga : Saga<BridgeAggregate<User>.WithId<Guid>>
                          , IAmStartedByMessages<RegisterUser>
                          , IHandleMessages<RenameUser>
    {
        public Task Handle(RegisterUser message, IMessageHandlerContext context)
        {
            User = User.Register(message.UserId, message.UserName);
            return context.PublishDomainEventsOf(User);
        }

        public Task Handle(RenameUser message, IMessageHandlerContext context)
        {
            User.Rename(message.NewUserName);
            return context.PublishDomainEventsOf(User);
        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<BridgeAggregate<User>.WithId<Guid>> mapper)
        {
            mapper.ConfigureMapping<RegisterUser>(m => m.UserId).ToSaga(data => data.AggregateRootId);
            mapper.ConfigureMapping<RenameUser>(m => m.UserId).ToSaga(data => data.AggregateRootId);
        }

        protected User User
        {
            get => Data.AggregateRoot;
            set => Data.AggregateRoot = value;
        }
    }
}
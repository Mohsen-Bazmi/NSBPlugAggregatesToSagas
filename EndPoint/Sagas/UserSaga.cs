using System.Threading.Tasks;
using Commands;
using Domain;
using EndPoint.NservicebusExtensions;
using NServiceBus;

namespace EndPoint.Sagas
{
    public class UserSaga : Saga<UserSagaData>
                          , IAmStartedByMessages<RegisterUser>
                          , IHandleMessages<RenameUser>
    {
        public Task Handle(RegisterUser message, IMessageHandlerContext context)
        {
            User = User.Register(message.UserId);
            return context.PublishDomainEventsOf(User);
        }

        public Task Handle(RenameUser message, IMessageHandlerContext context)
        {
            User.Rename(message.NewUserName);
            return context.PublishDomainEventsOf(User);
        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<UserSagaData> mapper)
        {
            mapper.ConfigureMapping<RegisterUser>(m => m.UserId).ToSaga(s => s.AggregateRootId);
            mapper.ConfigureMapping<RenameUser>(m => m.UserId).ToSaga(s => s.AggregateRootId);
        }

        protected User User
        {
            get => Data.AggregateRoot;
            set => Data.AggregateRoot = value;
        }
    }
}
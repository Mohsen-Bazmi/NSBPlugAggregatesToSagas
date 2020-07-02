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
    //----------------------------------------------------------------------------------
    //----------------------------------------------------------------------------------
    //----------------------------------------------------------------------------------
    // public class UserSagaData2 : ContainSagaData
    // {
    //     public System.Guid UserId { get; set; }
    //     public string Name { get; set; } = "Anonymous";
    // }
    // public class UserSaga2 : Saga<UserSagaData2>
    //                       , IAmStartedByMessages<RegisterUser>
    //                       , IHandleMessages<RenameUser>
    // {
    //     public Task Handle(RegisterUser message, IMessageHandlerContext context)
    //     {
    //         Data.UserId = message.UserId;
    //         return context.Publish(new Events.UserRegistered { Id = message.UserId });
    //     }

    //     public Task Handle(RenameUser message, IMessageHandlerContext context)
    //     {
    //         Data.Name = message.NewUserName;
    //         return Task.CompletedTask;
    //     }

    //     protected override void ConfigureHowToFindSaga(SagaPropertyMapper<UserSagaData2> mapper)
    //     {
    //         mapper.ConfigureMapping<RegisterUser>(m => m.UserId).ToSaga(s => s.UserId);
    //         mapper.ConfigureMapping<RenameUser>(m => m.UserId).ToSaga(s => s.UserId);
    //     }
    // }
}
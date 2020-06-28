using System;
using Domain;

namespace EndPoint.Sagas
{
    public class UserSagaData : WrapTheAggregate<User>
    {
        protected override User CreateAggregateRoot(Guid id)
        => User.Register(id);
    }
}
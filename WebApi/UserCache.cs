using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using NServiceBus;

namespace WebApi
{
    public class UserCache : IHandleMessages<Events.UserRegistered>
    {
        public List<UserViewModel> RegisteredUsers { get; set; } = new List<UserViewModel>();

        public Task Handle(Events.UserRegistered message, IMessageHandlerContext _)
        {
            RegisteredUsers.Add(new UserViewModel { Id = message.Id });
            return Task.CompletedTask;
        }
    }
}
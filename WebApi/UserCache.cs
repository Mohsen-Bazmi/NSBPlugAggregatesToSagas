using System.Collections.Generic;
using System.Linq;
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
            if (RegisteredUsers.Any(user => user.Id == message.Id))
                return Task.CompletedTask;
            RegisteredUsers.Add(new UserViewModel
            {
                Id = message.Id,
                Name = message.Name
            });
            return Task.CompletedTask;
        }
    }
}
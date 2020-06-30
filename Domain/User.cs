using System;

namespace Domain
{
    public class User : AggregateRoot
    {
        public static User Register(Guid id)
        => new User(id);

        private User(Guid id) : base(id)
        => AppendEvent(new Events.UserRegistered { Id = id });

        public void Rename(string newUserName)
        {
            Name = newUserName;
        }
        public string Name { get; private set; } = "Anonymous";
    }
}
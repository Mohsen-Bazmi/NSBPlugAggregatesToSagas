using System;

namespace Domain
{
    public sealed class User : AggregateRoot<Guid>
    {
        public static User Register(Guid id, string name)
        => new User(id, name);

        private User(Guid id, string name) : base(id)
        {
            Name = name;
            AppendEvent(new Events.UserRegistered
            {
                Id = id,
                Name = Name
            });
        }

        public void Rename(string newUserName)
        {
            Name = newUserName;
        }
        public string Name { get; private set; }
    }
}
using System;

namespace Domain
{
    public static class Events
    {
        public class UserRegistered
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }
    }
}
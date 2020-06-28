using System;

namespace Commands
{
    public class RenameUser
    {
        public Guid UserId { get; set; }
        public string NewUserName { get; set; }
    }
}
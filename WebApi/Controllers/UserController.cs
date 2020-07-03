using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NServiceBus;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase

    {
        readonly IMessageSession session;
        readonly UserCache users;
        readonly ILogger<UserController> logger;
        public UserController(IMessageSession session, UserCache users, ILogger<UserController> logger)
        {
            this.session = session;
            this.users = users;
            this.logger = logger;
        }

        [HttpGet]
        public IEnumerable<UserViewModel> Get()
        => users.RegisteredUsers;

        [HttpPost("{userName}")]
        public async Task<IActionResult> Register(string userName)
        {
            await session.Send(new Commands.RegisterUser
            {
                UserId = Guid.NewGuid(),
                UserName = userName
            }).ConfigureAwait(false);
            return RedirectToAction(nameof(Get));
        }

        [HttpPost("Rename")]
        public async Task<IActionResult> Rename(UserRenameDto dto)
        {
            await session.Send(new Commands.RenameUser
            {
                UserId = Guid.NewGuid(),
                NewUserName = dto.NewName
            }).ConfigureAwait(false);
            return RedirectToAction(nameof(Get));
        }
    }
}

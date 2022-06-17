using ChatServer.Business.Context;
using ChatServer.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatServer.Controller
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ChatContext _context;
        private readonly ILogger<UserController> _logger;
        public UserController(ILogger<UserController> logger, ChatContext dbContext)
        {
            _context = dbContext;
            _logger = logger;
        }

        [HttpGet("Login")]
        public User Login(string username, string password)
        {
            using (_context)
            {
                var user = _context.Users.FirstOrDefault(x => x.UserName == username && x.Password == password);
                if (user != null)
                {
                    if (user.UserName.Equals(username) && user.Password.Equals(password))
                    {
                        return user;
                    }
                }
                return null;
            }
        }

        [HttpGet("GetUser")]
        public User GetUser(string userid)
        {
            using (_context)
            {
                var user = _context.Users.FirstOrDefault(x => x.Id == userid);
                if (user != null)
                {
                    return user;
                }
                return null;
            }
        }

    }
}

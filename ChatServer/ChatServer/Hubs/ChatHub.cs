using ChatServer.Business.Context;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatServer.Model;
using System.Collections.Concurrent;

namespace ChatServer.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ILogger<ChatHub> _logger;
        private readonly ChatContext _context;
        private readonly static Dictionary<string, string> _ConnectionsMap = new Dictionary<string, string>();
        public ChatHub(ILogger<ChatHub> logger, ChatContext dbContext)
        {
            _logger = logger;
            _context = dbContext;
        }

        public async Task<bool> Login(string userid)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == userid);
            if (user != null)
            {
                if (!_ConnectionsMap.ContainsKey(userid))
                {
                    _ConnectionsMap.Add(userid, Context.ConnectionId);
                    await Clients.All.SendAsync("LoginMessage", _ConnectionsMap);
                    return true;
                }
            }

            return false;
        }

        public async Task<bool> LogOut(string userid)
        {
            if (_ConnectionsMap.ContainsKey(userid))
            {
                _ConnectionsMap.Remove(userid);
                await Clients.All.SendAsync("LogoutMessage", userid);
            }

            return true;
        }

        public async void SendMessage(string touserid, string fromuserid, string message)
        {
            if (_ConnectionsMap.ContainsKey(touserid))
            {
                var conectionid = _ConnectionsMap[touserid];
                await Clients.Client(conectionid).SendAsync("ReceiveClientMessage", touserid, message);
                await Clients.Caller.SendAsync("ReceiveCallerMessage", touserid , message);
            }
        }
    }
}

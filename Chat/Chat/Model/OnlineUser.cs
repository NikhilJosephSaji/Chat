using Chat.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.ViewModel
{
    public class OnlineUser
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
        public bool HasSentNewMessage { get; set; }
        public bool IsTyping { get; set; }

        public UserChat UserChats { get; set; }
    }
}

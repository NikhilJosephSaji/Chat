using System;

namespace Chat.ViewModel
{
    public class OnlineUser
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
        public bool HasSentNewMessage { get; set; }
        public bool IsTyping { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.Model
{
    public class UserChat
    {
        public string ChatUserImage { get; set; }
        public string Picture { get; set; }
        public string Author { get; set; }
        public DateTime Time { get; set; }
        public string Message { get; set; }
        public bool IsNativeSender { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.Constants
{
    public class ChatConstants
    {
        public const string Login = "/User/Login?username={0}&password={1}";
        public const string GetUser = "/User/GetUser?userid={0}";
        public const string PostUser = "User/Register";
        public const string HTTPURL = "https://localhost:44386";
        public const string URLCHAT = "https://localhost:44386/chathub";
    }
}

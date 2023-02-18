using System;

namespace ChatApp_SignalR.Models
{
    public class ChatListData
    {
        public string ContactName { get; set; }
        public Uri ContactPhoto { get; set; }
        public string Message { get; set; }
        public string LastMessageSentTime { get; set; }
    }
}